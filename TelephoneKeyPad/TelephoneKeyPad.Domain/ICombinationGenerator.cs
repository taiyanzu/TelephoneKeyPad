using System.Collections.Generic;

namespace TelephoneKeyPad.Domain
{
    public interface ICombinationGenerator
    {
        IEnumerable<string> Generate(string phoneNumber);
        int TotalItemCount(string phoneNumber);
        IEnumerable<string> GetPageItems(string phoneNumber, int page);
    }
}
