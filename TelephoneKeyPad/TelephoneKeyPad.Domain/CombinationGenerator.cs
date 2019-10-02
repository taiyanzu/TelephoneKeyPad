using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneKeyPad.Domain
{
    public class CombinationGenerator : ICombinationGenerator
    {
        public string PhoneNumber { get; }
        private readonly Keypad _keypad;
        private readonly char[] _current;
        private readonly object lockObject = new object();

        public CombinationGenerator(Keypad keypad)
        {
            keypad.ThrowsOnNull(nameof(keypad));
            _keypad = keypad;
        }

        private string Next()
        {
            lock (lockObject)
            {
                for (var i = _current.Length - 1; i >= 0; i--)
                {
                    if (_keypad.Symbols.TryGetValue(_current[i], out LinkedSymbol key)
                        && key.Next != char.MinValue)
                    {
                        // increment the current key, and reset all the keys after it
                        _current[i] = key.Next;
                        for (var j = i + 1; j <= _current.Length - 1; j++)
                        {
                            _current[j] = _keypad.Symbols[_current[j]].MainDigit;
                        }
                        return new string(_current);
                    }
                }
            }
            return null;
        }


        private IEnumerable<string> NextNumbers()
        {
            var data = Next();
            while (data != null)
            {
                yield return data;
                data = Next();
            }
        }

        //IEnumerator<string> IEnumerable<string>.GetEnumerator()
        //{
        //    return NextNumbers().GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.NextNumbers().GetEnumerator();
        //}

        private int GetNextPosition(char[] array)
        {
            for (var i = array.Length - 1; i >= 0; i--)
            {
                var next = _keypad.NextSymbol(array[i]);
                if (next != char.MinValue) return i;
            }
            return -1;
        }

        public IEnumerable<string> Generate(string phoneNumber)
        {
            phoneNumber.ThrowsOnNull(nameof(phoneNumber));
            if (!_keypad.CanAccept(phoneNumber)) yield break;

            var result = phoneNumber.ToCharArray();
            // Scans the array from right to left, find the next positition to advance
            var position = GetNextPosition(result);
            while (position >= 0)
            {
                for (var i = position + 1; i < result.Length; i++)
                {
                    // When advancing a position, reset all the symbols at the right side
                    var baseDigit = _keypad.Symbols[result[i]].MainDigit;
                    result[i] = baseDigit;
                }
                result[position] = _keypad.NextSymbol(result[position]);

                position = GetNextPosition(result);

                yield return new string(result);
            }
        }

        public int TotalItemCount(string phoneNumber)
        {
            phoneNumber.ThrowsOnNull(nameof(phoneNumber));

            int count = 1;
            bool multiplied = false;
            foreach (var digit in phoneNumber)
            {
                if (_keypad.KeyGroups.TryGetValue(digit, out var keyGroup) && keyGroup.Letters.Length > 1)
                {
                    count *= keyGroup.Letters.Length;
                    multiplied = true;
                }
            }

            return multiplied ? count : 0;
        }

        public IEnumerable<string> GetPageItems(string phoneNumber, int page)
        {
            throw new NotImplementedException();
        }
    }
}
