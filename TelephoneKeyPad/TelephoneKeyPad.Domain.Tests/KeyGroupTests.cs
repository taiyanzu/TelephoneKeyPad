using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TelephoneKeyPad.Domain.Tests
{
    [TestClass]
    public class KeyGroupTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeyGroup_Constructor_CsvCannotBeNull()
        {
            var sut = new KeyGroup(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KeyGroup_Constructor_CsvCannotBeEmpty()
        {
            var sut = new KeyGroup(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KeyGroup_Constructor_CsvCannotHaveDuplicate()
        {
            var sut = new KeyGroup("2ABAC");
        }

        [TestMethod]
        public void KeyGroup_Constructor_CsvParsedCorrectly()
        {
            var sut = new KeyGroup("2ABC");
            Assert.AreEqual('2', sut.MainDigit);
            Assert.AreEqual(3, sut.Letters.Length);
            Assert.AreEqual('A', sut.Letters[0]);
            Assert.AreEqual('B', sut.Letters[1]);
            Assert.AreEqual('C', sut.Letters[2]);
        }

        [TestMethod]
        public void KeyGroup_ToLinkedSymbols_WithoutLetters_OK()
        {
            var sut = new KeyGroup("0");
            var symbols = sut.ToLinkedSymbols().ToArray();
            Assert.AreEqual(1, symbols.Length);

            var symbol = symbols[0];
            Assert.AreEqual('0', symbol.Value);
            Assert.AreEqual('0', symbol.MainDigit);
            Assert.AreEqual(char.MinValue, symbol.Previous);
            Assert.AreEqual(char.MinValue, symbol.Next);

        }

        [TestMethod]
        public void KeyGroup_ToLinkedSymbols_WithLetters_OK()
        {
            var sut = new KeyGroup("2ABC");
            var symbols = sut.ToLinkedSymbols().ToArray();
            Assert.AreEqual(4, symbols.Length);

            foreach (var s in symbols)
            {
                Assert.AreEqual('2', s.MainDigit);
            }

            var digit = symbols[0];
            Assert.AreEqual(char.MinValue, digit.Previous);
            Assert.AreEqual('2', digit.Value);
            Assert.AreEqual('A', digit.Next);

            var firstLetter = symbols[1];
            Assert.AreEqual('2', firstLetter.Previous);
            Assert.AreEqual('A', firstLetter.Value);
            Assert.AreEqual('B', firstLetter.Next);

            var secondLetter = symbols[2];
            Assert.AreEqual('A', secondLetter.Previous);
            Assert.AreEqual('B', secondLetter.Value);
            Assert.AreEqual('C', secondLetter.Next);

            var lastLetter = symbols[3];
            Assert.AreEqual('B', lastLetter.Previous);
            Assert.AreEqual('C', lastLetter.Value);
            Assert.AreEqual(char.MinValue, lastLetter.Next);
        }


    }
}
