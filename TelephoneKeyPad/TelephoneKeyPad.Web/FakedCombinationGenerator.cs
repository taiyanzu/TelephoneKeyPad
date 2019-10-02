using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneKeyPad.Domain;

namespace TelephoneKeyPad.Web
{
    public class FakedCombinationGenerator : ICombinationGenerator
    {
        private const int MAX = 1000;
        public int TotalItemCount()
        {
            return MAX;
        }

        public IEnumerable<string> Generate()
        {
            for (var i = 0; i < MAX; i++)
            {
                yield return i.ToString() + "-" + Guid.NewGuid().ToString();
            }
        }

        public IEnumerable<string> GetPageItems()
        {
            return Enumerable.Range(1, 10).Select(_ => Guid.NewGuid().ToString());
        }
    }
}
