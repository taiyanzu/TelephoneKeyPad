using System;
using System.Collections.Generic;
using System.Linq;

namespace TelephoneKeyPad.Domain
{
    public class KeyGroup
    {
        private const string ERROR_IS_EMPTY = "{0} should not be empty.";
        private const string ERROR_CONTAINS_DUPLICATES = "{0} should not contain duplicates.";

        public char MainDigit { get; set; }
        public char[] Letters { get; set; }
        public KeyGroup(string csvKeyGroup)
        {
            csvKeyGroup.ThrowsOnNull(nameof(csvKeyGroup));
            if (csvKeyGroup.Length == 0)
            {
                throw new ArgumentException(string.Format(ERROR_IS_EMPTY, nameof(csvKeyGroup)));
            }

            if (csvKeyGroup.GroupBy(c => c).Any(g => g.Count() > 1))
            {
                throw new ArgumentException(string.Format(ERROR_CONTAINS_DUPLICATES, nameof(csvKeyGroup)));
            }

            MainDigit = csvKeyGroup[0];
            Letters = csvKeyGroup.Length == 1 ? new char[0] : csvKeyGroup.Substring(1).ToCharArray();
        }

        public IEnumerable<LinkedSymbol> ToLinkedSymbols()
        {
            yield return new LinkedSymbol
            {
                Value = char.Parse(MainDigit.ToString()),
                MainDigit = MainDigit,
                Previous = char.MinValue,
                Next = Letters.Length == 0 ? char.MinValue : Letters[0]
            };

            for (var i = 0; i < Letters.Length; i++)
            {
                yield return new LinkedSymbol
                {
                    Value = Letters[i],
                    MainDigit = MainDigit,
                    Previous = (i == 0) ? MainDigit : Letters[i - 1],
                    Next = (i == Letters.Length - 1) ? char.MinValue : Letters[i + 1]
                };
            }
        }

        public bool TryGetNextSymbol(char c, out LinkedSymbol symbol)
        {
            symbol = ToLinkedSymbols().FirstOrDefault(x => x.Value == c);
            return symbol != null;
        }
    }
}
