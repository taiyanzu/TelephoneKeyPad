using System;

namespace TelephoneKeyPad.Domain
{
    public static class Extensions
    {
        public static void ThrowsOnNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"{name} cannot be null.");
            }
        }
    }
}
