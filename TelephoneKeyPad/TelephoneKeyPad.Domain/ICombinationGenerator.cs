using System.Collections.Generic;

namespace TelephoneKeyPad.Domain
{
    public interface ICombinationGenerator
    {
        IEnumerable<string> Generate();
        int TotalItemCount();
        IEnumerable<string> GetPageItems();
    }
}
