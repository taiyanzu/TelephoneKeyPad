namespace TelephoneKeyPad.Domain
{
    public class LinkedSymbol
    {
        public char Value { get; internal set; }
        public char MainDigit { get; internal set; }
        public char Previous { get; internal set; }
        public char Next { get; internal set; }
    }
}
