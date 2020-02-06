using FibonnacciLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigOUnitTests
{
    [TestClass]
    public class FibonacciTests
    {
        [TestMethod]
        public void NaiveFibTest_Index4()
        {
            ulong answer = FibonacciCalculations.JustOneFib(4);
            Assert.IsTrue(answer == 3); 
        }

        [TestMethod]
        public void NaiveFibTest_Index0()
        {
            ulong answer = FibonacciCalculations.JustOneFib(0);
            Assert.IsTrue(answer == 0);
        }

        [TestMethod]
        public void NaiveFibTest_Index1()
        {
            ulong answer = FibonacciCalculations.JustOneFib(1);
            Assert.IsTrue(answer == 1);
        }

        public void FibWithTailRecursion_Index4()
        {
            ulong answer = FibonacciCalculations.GetFibUsingIntelligentRecursionHelper(4);
            Assert.IsTrue(answer == 3);
        }

        [TestMethod]
        public void FibFromSequence_Index4()
        {
            ulong answer = FibonacciCalculations.JustOneFibFaster(4);
            Assert.IsTrue(answer == 3);
        }

        [TestMethod]
        public void FibFromSequence_IndexLargeNumber()
        {
            ulong input = 50;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ulong answer = FibonacciCalculations.JustOneFibFaster(input);
            sw.Stop();
            Console.WriteLine($"Test took {sw.ElapsedMilliseconds} milliseconds. Input was {input} and answer was {answer}");
            Console.WriteLine($"Test took {sw.ElapsedTicks} ticks");
            Assert.IsTrue(sw.ElapsedTicks > 0);
            
        }

        [TestMethod]
        public void FibFromLoop_Index4()
        {
            ulong answer = FibonacciCalculations.JustOneFibSimpleLoop(4);
            Assert.IsTrue(answer == 3);
        }
        [TestMethod]
        public void FibFromLoop_IndexLargeNumber()
        {
            ulong input = 10000000;// 1000000; //100000;//40000;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // 10000 gives 1 msecond as does 12500
            ulong answer = FibonacciCalculations.JustOneFibSimpleLoop(input);
            sw.Stop();
            Console.WriteLine($"Test took {sw.ElapsedMilliseconds} milliseconds. Input was {input} and answer was {answer}");
            Console.WriteLine($"Test took {sw.ElapsedTicks} ticks");
            Assert.IsTrue(sw.ElapsedTicks > 0);

        }
    }
}
