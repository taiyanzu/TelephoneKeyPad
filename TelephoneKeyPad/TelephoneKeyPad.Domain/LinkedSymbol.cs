namespace TelephoneKeyPad.Domain
{
    /// <summary>
    /// Any alphanumeric character on a telephone keypad. 
    /// </summary>
    public class LinkedSymbol
    {
        public char Value { get; internal set; }
        public char MainDigit { get; internal set; }
        public char Previous { get; internal set; }
        public char Next { get; internal set; }
    }
}
