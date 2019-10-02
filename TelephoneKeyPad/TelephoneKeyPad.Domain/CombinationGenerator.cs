using System;
using System.Collections.Generic;

namespace TelephoneKeyPad.Domain
{
    public class CombinationGenerator : ICombinationGenerator
    {
        private readonly Keypad _keypad;

        public CombinationGenerator(Keypad keypad)
        {
            keypad.ThrowsOnNull(nameof(keypad));
            _keypad = keypad;
        }

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

        public IEnumerable<string> GetPageItems(string phoneNumber, int page, int pageSize)
        {
            // This is an optimization to get arbitrary page without enumerating through the generator
            throw new NotImplementedException();
        }
    }
}
