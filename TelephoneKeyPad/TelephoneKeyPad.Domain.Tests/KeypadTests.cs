using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TelephoneKeyPad.Domain.Tests
{
    [TestClass]
    public class KeypadTests
    {
        [TestMethod]
        public void Keypad_Constructor_UseE16KeyGroupByDefault()
        {
            var keypadE161 = Keypad.E161;
            var keypadDefaut = new Keypad();

            Assert.AreEqual(keypadE161.KeyGroups.Count, keypadDefaut.KeyGroups.Count);
            Assert.AreEqual(keypadE161.Symbols.Count, keypadDefaut.Symbols.Count);

            Assert.AreEqual(10, keypadDefaut.KeyGroups.Count);
            Assert.AreEqual(36, keypadDefaut.Symbols.Count);
        }

        [TestMethod]
        public void Keypad_Constructor_CustomKeyGroups_Works()
        {
            var keypad = new Keypad("0,1,2ABC");
            Assert.AreEqual(3, keypad.KeyGroups.Count);
            Assert.AreEqual(6, keypad.Symbols.Count);
        }

        [TestMethod]
        public void Keypad_Constructor_CustomKeyGroup_IgnoreSpaceInCsv()
        {
            var keypad = new Keypad("0, 1, 2A BC");
            Assert.AreEqual(3, keypad.KeyGroups.Count);
            Assert.AreEqual(6, keypad.Symbols.Count);
        }

        [TestMethod]
        public void Keypad_NextSymbol_ReturnsCorrectValue()
        {
            var keypad = new Keypad("2ABC");

            Assert.AreEqual('A', keypad.NextSymbol('2'));
            Assert.AreEqual('B', keypad.NextSymbol('A'));
            Assert.AreEqual('C', keypad.NextSymbol('B'));
            Assert.AreEqual(char.MinValue, keypad.NextSymbol('C'));
        }

        [TestMethod]
        public void Keypad_NextSymbol_SpecialGroups()
        {
            var keypad = new Keypad("0");
            Assert.AreEqual(char.MinValue, keypad.NextSymbol('0'));

            keypad = new Keypad("1");
            Assert.AreEqual(char.MinValue, keypad.NextSymbol('1'));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Keypad_NextSymbol_InvalidKey_Exception()
        {
            var keypad = new Keypad("2ABC");
            var test = keypad.NextSymbol('3');
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Keypad_NextSymbol_LowerCase_Exception()
        {
            var keypad = new Keypad("2ABC");
            var test = keypad.NextSymbol('a');
        }

        [DataTestMethod]
        [DataRow('0', char.MinValue)]
        [DataRow('1', char.MinValue)]
        [DataRow('2', 'A')]
        [DataRow('A', 'B')]
        [DataRow('B', 'C')]
        [DataRow('C', char.MinValue)]

        [DataRow('3', 'D')]
        [DataRow('D', 'E')]
        [DataRow('E', 'F')]
        [DataRow('F', char.MinValue)]

        [DataRow('4', 'G')]
        [DataRow('G', 'H')]
        [DataRow('H', 'I')]
        [DataRow('I', char.MinValue)]

        [DataRow('5', 'J')]
        [DataRow('J', 'K')]
        [DataRow('K', 'L')]
        [DataRow('L', char.MinValue)]

        [DataRow('6', 'M')]
        [DataRow('M', 'N')]
        [DataRow('N', 'O')]
        [DataRow('O', char.MinValue)]

        [DataRow('7', 'P')]
        [DataRow('P', 'Q')]
        [DataRow('Q', 'R')]
        [DataRow('R', 'S')]
        [DataRow('S', char.MinValue)]

        [DataRow('8', 'T')]
        [DataRow('T', 'U')]
        [DataRow('U', 'V')]
        [DataRow('V', char.MinValue)]

        [DataRow('9', 'W')]
        [DataRow('W', 'X')]
        [DataRow('X', 'Y')]
        [DataRow('Y', 'Z')]
        [DataRow('Z', char.MinValue)]
        public void Keypad_E16KeyGroup_DefinedCorrectly(char value, char expected)
        {
            var keypad = new Keypad(Keypad.E161_PHONE_KEY_GROUPS);
            Assert.AreEqual(expected, keypad.NextSymbol(value));
        }

        [TestMethod]
        public void KeyPad_CannAccept_NullOrEmpty_ReturnsFalse()
        {
            var keypad = new Keypad(Keypad.E161_PHONE_KEY_GROUPS);
            Assert.IsFalse(keypad.CanAccept(null));
            Assert.IsFalse(keypad.CanAccept(string.Empty));
            Assert.IsFalse(keypad.CanAccept(" "));
        }

        [TestMethod]
        public void KeyPad_CannAccept_InvalidCharacters_ReturnsFalse()
        {
            var keypad = new Keypad(Keypad.E161_PHONE_KEY_GROUPS);
            Assert.IsFalse(keypad.CanAccept("222-333-1111"));
            Assert.IsFalse(keypad.CanAccept("222 333 1111"));
            Assert.IsFalse(keypad.CanAccept("(222)3331111"));
        }

        [TestMethod]
        public void KeyPad_CannAccept_Alphabets_OK()
        {
            var keypad = new Keypad(Keypad.E161_PHONE_KEY_GROUPS);
            Assert.IsTrue(keypad.CanAccept("ABCDEFGHI"));
            Assert.IsTrue(keypad.CanAccept("AA33S221"));
            Assert.IsTrue(keypad.CanAccept("A0000000"));
        }

        [TestMethod]
        public void KeyPad_CanAccept_CustomKeyPad_Works()
        {
            var keypad = new Keypad("0,1,2,3");
            Assert.IsTrue(keypad.CanAccept("1"));
            Assert.IsTrue(keypad.CanAccept("11"));
            Assert.IsTrue(keypad.CanAccept("11111"));
            Assert.IsTrue(keypad.CanAccept("230"));

            Assert.IsFalse(keypad.CanAccept("A"));
            Assert.IsFalse(keypad.CanAccept("400"));
        }

        [TestMethod]
        public void KeyPad_CannAccept_AlphabetsNotRegistered_ReturnsFalse()
        {
            var keypad = new Keypad("0,1ABC");

            Assert.IsFalse(keypad.CanAccept("D"));
        }
    }
}
