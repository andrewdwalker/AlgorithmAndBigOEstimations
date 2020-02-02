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
        [TestMethod]
        public void TestLinearAlgorithm()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LinearAlgorithm, new List<double>() { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
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
        uint LinearAlgorithm(uint n)
        {
            for (uint i = 0; i < n; i++)
            {
                Thread.Sleep(2);
                //uint y = n - i; // dummy calculation
            }
            return n;
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
