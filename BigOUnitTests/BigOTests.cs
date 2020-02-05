using BigOEstimator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //[TestMethod]
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
        public void SanityTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //LinearAlgorithm1(500);
            //stopwatch.Stop();
            //Console.WriteLine($"LinearAlgorithm1 took {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            LinearAlgorithm2(100000000);
            stopwatch.Stop();
            Console.WriteLine($"LinearAlgorithm2 took {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            //LogNAlgorithm(9000000000000000000);
            LogNAlgorithm(ulong.MaxValue);
            stopwatch.Stop();
            Console.WriteLine($"LogNAlgorithm took {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            QuadraticAlgorithm(5000);
            stopwatch.Stop();
            Console.WriteLine($"QuadraticAlgorithm took {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            CubicAlgorithm1(1000);
            stopwatch.Stop();
            Console.WriteLine($"CubicAlgorithm1 took {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            CubicAlgorithm2(1000);
            stopwatch.Stop();
            Console.WriteLine($"CubicAlgorithm2 took {stopwatch.ElapsedMilliseconds}");
        }
        [TestMethod]
        public void TestLinearAlgorithm2()
        {
            Evaluator evaluator = new Evaluator();
            List<double> lst = new List<double>();



            // THIS WORKS
            //for (Int64 i = 100000000; i < 1000000000; i = i + 10000034)
            //{
            //    lst.Add((double)i);
            //}
            for (Int64 i = 100000000; i < 500000000; i = i + 10000030)
            {
                lst.Add((double)i);
            }
           


            var result = evaluator.Evaluate(LinearAlgorithm2, lst, false);
           
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.N.ToString());
        }

        [TestMethod]
        public void TestQuadraticAlgorithm()
        {
            List<double> lst = new List<double>();
            Evaluator evaluator = new Evaluator();
            //var result = evaluator.Evaluate(QuadraticAlgorithm, new List<double>()
            //{ 1000,1021, 1065, 1300, 1423, 1599,
            //    1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
            //    4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            for (Int64 i = 5000; i < 10000; i = i + 100)
            {
                lst.Add((double)i);
            }

            var result = evaluator.Evaluate(QuadraticAlgorithm, lst, false);
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.NSquared.ToString());
        }

        [TestMethod]
        public void TestCubicAlgorithm1()
        {
            List<double> lst = new List<double>();
            Evaluator evaluator = new Evaluator();
            
            for (Int64 i = 100; i < 600; i = i + 35)
            {
                lst.Add((double)i);
            }

            var result = evaluator.Evaluate(CubicAlgorithm1, lst, false);
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.NCubed.ToString());
        }

        [TestMethod]
        public void TestCubicAlgorithm2()
        {
            List<double> lst = new List<double>();
            Evaluator evaluator = new Evaluator();

            //for (Int64 i = 1000; i < 9000; i = i + 400)
            for (Int64 i = 100; i < 600; i = i + 35)
            {
                lst.Add((double)i);
            }

            var result = evaluator.Evaluate(CubicAlgorithm2, lst, false);
            var minKey = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Assert.IsTrue(minKey.ToString() == FunctionEnum.NCubed.ToString());
        }

        [TestMethod]      
        public void TestLogNAlgorithm()
        {
            List<double> lst = new List<double>();
            Evaluator evaluator = new Evaluator();
            //var result = evaluator.Evaluate(LogNAlgorithm, new List<double>()
            //{ 1000,1021, 1065, 1300, 1423, 1599,
            //    1683, 1722, 1822, 2000, 2050, 2090, 2500, 3000, 3100, 3109, 3500,
            //    4000, 4022, 4089, 4122, 4199, 4202, 4222, 5000 });
            for (ulong i = 1000000000000; i < 10000000000000; i = i + 50000000000)
            {
                lst.Add((double)i);
            }

            var result = evaluator.Evaluate(LogNAlgorithm, lst);

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
        ulong LinearAlgorithm1(ulong n)
        {
            for (ulong i = 0; i < n; i++)
            {
                Thread.Sleep(1);
                //uint y = n - i; // dummy calculation
            }
            return (ulong) _randomNumber.Next(1000);
        }

        ulong LinearAlgorithm2(ulong n)
        {
            ulong z = 0;
           // uint returnValue = 7;
            for (ulong i = 0; i < n; i++)
            {
                //Thread.Sleep(2);
                //double y = _randomNumber.NextDouble()*100.0; // dummy calculation
                z = n + i;
               

            }
            return (ulong)Math.Log(z);
        }

        /// <summary>
        /// Should run in quadratic, or n^2 time
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        ulong QuadraticAlgorithm(ulong n)
        {
            ulong z = 0;
            for (ulong i = 0; i < n; i++)
            {
                for (ulong j = 0; j < n; j++)
                {
                    z = i + j;
                }
            }
            return (ulong)Math.Log(z);//return n * n;
        }

        ulong CubicAlgorithm1(ulong n)
        {
            ulong z = 0;
            for (ulong i = 0; i < n*n*n; i++)
            {
                z = z + i;
            }
            return (ulong)Math.Log(z);//return n * n;
        }

        ulong CubicAlgorithm2(ulong n)
        {
            ulong z = 0;
            for (ulong i = 0; i < n; i++)
            {
                for (ulong j = 0; j < n; j++)
                {
                    for (ulong k = 0; k < n; k++)
                    {
                        z = z + k;
                    }
                }
            }
            return (ulong)Math.Log(z);
        }

        /// <summary>
        /// Idea suggested by:
        /// https://softwareengineering.stackexchange.com/questions/146021/determining-if-an-algorithm-is-o-log-n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        ulong LogNAlgorithm(ulong n)
        {
            double test1 = Math.Log(n);
            double test2 = Math.Log10(n);
            double z = 0;
            for (ulong i = 0; i < Math.Log(n); i++)
            {
                Thread.Sleep(20);
                z = Math.Sin(z)*Math.Cos(z)*Math.Sqrt(Math.Sin(z)) + Math.Sin(i) + Math.Cos(i)*Math.Sin(i*i*i) + Math.Sin(n);
            }
            return (ulong)Math.Log(z);
        }
        #endregion
    }
}
