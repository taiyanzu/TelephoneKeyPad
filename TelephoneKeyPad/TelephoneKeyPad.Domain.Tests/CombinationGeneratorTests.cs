using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TelephoneKeyPad.Domain.Tests
{
    [TestClass]
    public class CombinationGeneratorTests
    {
        private const string TEST_PHONE_NUMBER = "2225559876";
        private Keypad StandardKeypad => Keypad.E161;

        #region Generate() Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Generator_Generate_RequiresInputhoneNumber()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            generator.Generate(null).ToList();
        }

        [TestMethod]
        public void Generator_IsIEnumerable()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            var combinations = generator.Generate(TEST_PHONE_NUMBER).Take(10).ToList();
            Assert.AreEqual(10, combinations.Count());
            combinations.ForEach(Console.WriteLine);
        }

        [TestMethod]
        public void Generator_InvalidPhoneNumber_ReturnsNull()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            var test = generator.Generate("(ABC)").FirstOrDefault();
            Assert.IsNull(test);
        }

        [TestMethod]
        public void Generator_NoCombinationPossible_ReturnsNull()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            var test = generator.Generate("0000000000").FirstOrDefault();
            Assert.IsNull(test);
        }

        [TestMethod]
        public void Generator_LastSymbolChanges_OK()
        {
            var phoneNumber = "3015559876";
            var generator = new CombinationGenerator(StandardKeypad);
            var combinations = generator.Generate(phoneNumber).Take(3).ToArray();
            Assert.AreEqual(combinations[0], "301555987M");
            Assert.AreEqual(combinations[1], "301555987N");
            Assert.AreEqual(combinations[2], "301555987O");
        }

        [TestMethod]
        public void Generator_SecondToLastSymbolChanges_OK()
        {
            var phoneNumber = "3015559860";
            var generator = new CombinationGenerator(StandardKeypad);
            var nextNumbers = generator.Generate(phoneNumber).Take(2).ToArray();
            Assert.AreEqual(nextNumbers[0], "30155598M0");
            Assert.AreEqual(nextNumbers[1], "30155598N0");
        }

        [TestMethod]
        public void Generator_LastTwoSymbolsChange_OK()
        {
            var phoneNumber = "301555987O";
            var generator = new CombinationGenerator(StandardKeypad);
            var combinations = generator.Generate(phoneNumber).Take(5).ToArray();
            Assert.AreEqual(combinations[0], "30155598P6");
            Assert.AreEqual(combinations[1], "30155598PM");
            Assert.AreEqual(combinations[2], "30155598PN");
            Assert.AreEqual(combinations[3], "30155598PO");
        }

       

        [TestMethod]
        public void Generator_AllCombinations_Verified()
        {
            var phoneNumber = "2";
            var generator = new CombinationGenerator(StandardKeypad);
            var nextNumbers = generator.Generate(phoneNumber).Take(1000).ToArray();
            Assert.AreEqual(3, nextNumbers.Length);
            Assert.AreEqual("A", nextNumbers[0]);
            Assert.AreEqual("B", nextNumbers[1]);
            Assert.AreEqual("C", nextNumbers[2]);
        }

        [TestMethod]
        public void Generator_ThreadSafe_ParallelReaders()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            var threadIdByNumber = new ConcurrentDictionary<string, int>();

            generator
                .Generate(TEST_PHONE_NUMBER)
                .Take(100)
                .AsParallel()
                .ForAll(n =>
                threadIdByNumber.TryAdd(n, Thread.CurrentThread.ManagedThreadId));

            Assert.AreEqual(100, threadIdByNumber.Count());
            foreach (var kvp in threadIdByNumber.OrderBy(x => x.Key))
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}");
            }
        }

        [TestMethod]
        public void Generator_ThreadSafe_TaskReaders()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            var threadCount = 1000;
            var batchSize = 100;

            var tasks = Enumerable.Range(1, threadCount)
                .Select(_ =>
                    Task.Factory.StartNew(() =>generator.Generate(TEST_PHONE_NUMBER).Take(batchSize).ToList())
                    )
                .ToArray();

            Task.WaitAll(tasks);


            var sequentialGenerator = new CombinationGenerator(StandardKeypad);
            var sequence = sequentialGenerator.Generate(TEST_PHONE_NUMBER).Take(threadCount * batchSize).ToList();

            for (var i = 0; i<batchSize; i++)
            {
                foreach(var task in tasks)
                {
                    Assert.AreEqual(sequence[i], task.Result[i]);
                }
            }
        }

        #endregion

        #region TotalItemCount Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Generator_TotalItemCount_RequiresInputhoneNumber()
        {
            var generator = new CombinationGenerator(StandardKeypad);
            generator.TotalItemCount(null);
        }

        [TestMethod]
        public void Generator_TotalItemCount_CorrectWithOneDigit()
        {
            var generator = new CombinationGenerator(StandardKeypad);

            Assert.AreEqual(0, generator.TotalItemCount("0"));
            Assert.AreEqual(0, generator.TotalItemCount("1"));
            Assert.AreEqual(3, generator.TotalItemCount("2"));
            Assert.AreEqual(3, generator.TotalItemCount("3"));
            Assert.AreEqual(3, generator.TotalItemCount("4"));
            Assert.AreEqual(3, generator.TotalItemCount("5"));
            Assert.AreEqual(3, generator.TotalItemCount("6"));
            Assert.AreEqual(4, generator.TotalItemCount("7"));
            Assert.AreEqual(3, generator.TotalItemCount("8"));
            Assert.AreEqual(4, generator.TotalItemCount("9"));
        }

        [TestMethod]
        public void Generator_TotalItemCount_CorrectWithMoreDigits()
        {
            var generator = new CombinationGenerator(StandardKeypad);

            Assert.AreEqual(0, generator.TotalItemCount("000"));
            Assert.AreEqual(0, generator.TotalItemCount("101"));

            Assert.AreEqual(3, generator.TotalItemCount("200"));
            Assert.AreEqual(3, generator.TotalItemCount("113"));

            Assert.AreEqual(3 * 3 * 3, generator.TotalItemCount("222"));

            Assert.AreEqual(Math.Pow(3, 10), generator.TotalItemCount("5552224444"));
            Assert.AreEqual(Math.Pow(4, 10), generator.TotalItemCount("7779997777"));
        }

        #endregion
    }
}
