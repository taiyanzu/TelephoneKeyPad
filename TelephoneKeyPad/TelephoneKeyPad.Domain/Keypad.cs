using System.Collections.Generic;
using System.Linq;

namespace TelephoneKeyPad.Domain
{
    /// <summary>
    /// A telephone keypad consisting groups of keys
    /// </summary>
    public class Keypad
    {
        // https://en.wikipedia.org/wiki/E.161
        public const string E161_PHONE_KEY_GROUPS = "0,1,2ABC,3DEF,4GHI,5JKL,6MNO,7PQRS,8TUV,9WXYZ";

        public static Keypad E161 => new Keypad(E161_PHONE_KEY_GROUPS);

        public Dictionary<char, KeyGroup> KeyGroups { get; set; }
        public Dictionary<char, LinkedSymbol> Symbols { get; set; }

        public Keypad(string keyGroupsCsv = null)
        {
            // Use E161 as default keypad
            var groups = (keyGroupsCsv ?? E161_PHONE_KEY_GROUPS)
                .Replace(" ", string.Empty)
                .Split(',');

            KeyGroups = groups.Select(x => new KeyGroup(x)).ToDictionary(x => x.MainDigit);
            Symbols = KeyGroups.Values.SelectMany(x => x.ToLinkedSymbols()).ToDictionary(x => x.Value);
        }

        public char NextSymbol(char v) => Symbols[v].Next;

        public bool CanAccept(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;
            foreach(var x in phoneNumber)
            {
                if (!Symbols.ContainsKey(x)) return false;
            }
            return true;
        }
    }
}
