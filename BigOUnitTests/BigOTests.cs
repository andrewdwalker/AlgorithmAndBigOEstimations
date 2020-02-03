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
            var result = evaluator.Evaluate(LinearAlgorithm1, new List<double>()
            { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.N.ToString());
        }

        [TestMethod]
        public void TestLinearAlgorithm2()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LinearAlgorithm2, new List<double>()
                //{ 1000,1021, 1065, 1300, 1423, 1599,
                //1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                //4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            { 
                3000, 3100, 3109, 3112, 3117, 3200, 3211, 3216, 3219, 3232, 3500, 3666, 3777,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000, 6000,
                7000, 7500, 8000, 9255, 10050, 10090, 11000, 11200, 11500,
                12001, 13020, 14552, 15999, 19222
            });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.N.ToString());
        }

        [TestMethod]
        public void TestQuadraticAlgorithm()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(QuadraticAlgorithm, new List<double>()
            { 1000,1021, 1065, 1300, 1423, 1599,
                1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
                4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.NSquared.ToString());
        }

        [TestMethod]      
        public void TestLogNAlgorithm()
        {
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(LogNAlgorithm, new List<double>()
            { 1000,1021, 1065, 1300, 1423, 1599,
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
            return (uint) _randomNumber.Next(1000);
        }

        uint LinearAlgorithm2(uint n)
        {
            uint z = 0;
           // uint returnValue = 7;
            for (uint i = 0; i < n; i++)
            {
                //Thread.Sleep(2);
                //double y = _randomNumber.NextDouble()*100.0; // dummy calculation
                z = n + i;
                //if (y < 0.0005)
                //{
                //    returnValue = 1;
                //    //Console.WriteLine("y " + y + i);
                //}
                //else if (y < .05)
                //{
                //    returnValue = 2;
                //}
                //else if (y < .5)
                //{
                //    returnValue = 3;
                //}
                //else
                //{
                //    returnValue = 7;
                //}

            }
            return (uint)Math.Log(z);
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
            uint z = 0;
            for (uint i = 0; i < Math.Log(n); i++)
            {
                z = n + i;
            }
            return (uint)Math.Log(z);
        }
        #endregion
    }
}
