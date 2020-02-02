using BigOEstimator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigOUnitTests
{
    
    [TestClass]
    public class BigOTests
    {
        private Random _randomNumber = new Random();
        private double _randomDouble;

        [TestInitialize]
        public void Initialize()
        {
            _randomDouble = _randomNumber.NextDouble();
        }
        [TestMethod]
        public void TestLinearAlgorithm1()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LinearAlgorithm1, new List<double>() { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.N.ToString());
        }

        [TestMethod]
        public void TestLinearAlgorithm2()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LinearAlgorithm2, new List<double>() { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1691, 1692, 1696, 1699, 1705,1709, 1712, 1713, 1717, 1720,
                1722, 1822, 2000, 2050, 2090, 2500, 2666, 2700,2701, 2767, 2799, 2822, 2877,
                3000, 3100, 3109, 3112, 3117, 3200, 3211, 3216, 3219, 3232, 3500, 3666, 3777,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.N.ToString());
        }

        [TestMethod]
        public void TestQuadraticAlgorithm()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(QuadraticAlgorithm, new List<double>() { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.NSquared.ToString());
        }

        [TestMethod]      
        public void TestLogNAlgorithm()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LogNAlgorithm, new List<double>() { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.LogN.ToString());
        }

        #region "Algorithms"
        /// <summary>
        /// Just one for loop.  Should run in linear time
        /// i.e. Run time should be proportional to n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        uint LinearAlgorithm1(uint n)
        {
            for (uint i = 0; i < n; i++)
            {
                Thread.Sleep(1);
                //uint y = n - i; // dummy calculation
            }
            return n;
        }

        uint LinearAlgorithm2(uint n)
        {
            uint returnValue = 7;
            for (uint i = 0; i < n; i++)
            {
                //Thread.Sleep(2);
                double y = _randomNumber.NextDouble(); // dummy calculation
                if (y < 0.0005)
                {
                    returnValue = 1;
                    Console.WriteLine("y " + y + i);
                }
                else if (y < .05)
                {
                    returnValue = 2;
                }
                else if (y < .5)
                {
                    returnValue = 3;
                }
                else
                {
                    returnValue = 7;
                }


            }
            return returnValue;
        }

        /// <summary>
        /// Should run in quadratic, or n^2 time
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        uint QuadraticAlgorithm(uint n)
        {
            for (uint i = 0; i < n; i++)
            {
                for (uint j = 0; j < n; j++)
                {
                    uint z = i + j;
                }
            }
            return n * n;
        }

        /// <summary>
        /// Idea suggested by:
        /// https://softwareengineering.stackexchange.com/questions/146021/determining-if-an-algorithm-is-o-log-n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        uint LogNAlgorithm(uint n)
        {
            for (uint i = 0; i < Math.Log(n); i++)
            {
                uint z = n + i;
            }
            return (uint)Math.Log(n);
        }
        #endregion
    }
}
