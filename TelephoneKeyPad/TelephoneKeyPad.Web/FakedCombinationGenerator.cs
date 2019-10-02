using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneKeyPad.Domain;

namespace TelephoneKeyPad.Web
{
    [Obsolete("Used for development")]
    public class FakedCombinationGenerator : ICombinationGenerator
    {
        private const int MAX = 1000;
        public int TotalItemCount(string phoneNumber)
        {
            return MAX;
        }

        public IEnumerable<string> Generate(string phoneNumber)
        {
            for (var i = 0; i < MAX; i++)
            {
                yield return (i+1).ToString() + "-" + phoneNumber;
            }
        }

        public IEnumerable<string> GetPageItems(string phoneNumber, int page, int pageSize = 10)
        {
            return Enumerable.Range(1, pageSize).Select(_ => Guid.NewGuid().ToString());
        }
    }
}
