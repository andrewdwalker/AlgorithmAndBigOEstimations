using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Library to evaluate complexity of algorithm.
/// Pass in method and necessary data
/// There are methods to set the size of the test data
/// 
/// Evaluate for LogN, N, NLogN, N^2, N^3, 2^N
/// 
/// Should be able use ideas from
/// https://en.wikipedia.org/wiki/Polynomial_regression
/// to finish problem.  Next need matrix multiplication.
/// Or possibly use this:
/// https://www.codeproject.com/Articles/19032/C-Matrix-Library
/// or similar
/// </summary>
namespace BigOEstimator
{
    public enum FunctionEnum
    {
        Constant = 0,
        LogN = 1,
        N,
        NLogN,
        NSquared,
        NCubed,
        TwoToTheN
    }
    public class Evaluator
    {
        //private List<uint> _suggestedList = new List<uint>();
        private Dictionary<FunctionEnum, double> _results = new Dictionary<FunctionEnum, double>(); 
        public Evaluator()
        {
            //_suggestedList.Add(10);
            //_suggestedList.Add(20);
            //_suggestedList.Add(50);
            //_suggestedList.Add(100);
            
        }

        public Dictionary<FunctionEnum, double> Evaluate(Func<uint,uint> algorithm, IList<double> suggestedList)
        {
            Dictionary<FunctionEnum, double> results = new Dictionary<FunctionEnum, double>();
            //List<Tuple<uint, uint>> results = new List<Tuple<uint, uint>>();
            //Matrix<double> m = Matrix<double>.Build.Dense(5, 5, 0.0);
            Vector<double> answer = Vector<double>.Build.Dense(suggestedList.Count(), 0.0);
            for (int i = 0; i < suggestedList.Count(); i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = algorithm((uint) suggestedList[i]);
                stopwatch.Stop();
                answer[i] = stopwatch.ElapsedTicks;
                
                Console.WriteLine($"Answer for index {suggestedList[i]} is {answer[i]}");
            }

            // linear case - N
            results[FunctionEnum.N] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(suggestedList), answer, d => d);
            // quadratic case - NSquared
            results[FunctionEnum.NSquared] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(suggestedList), answer, d => (d*d));
            // cubic case - NCubed
            results[FunctionEnum.NCubed] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(suggestedList), answer, d => (d * d * d));
            // NLogN case - NLogN
            results[FunctionEnum.NLogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(suggestedList), answer, d => (d * Math.Log(d)));
            // LogN case - LogN
            results[FunctionEnum.LogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(suggestedList), answer, d => ( Math.Log(d)));

            // following few lines are useful for unit tests. You get this by hitting 'Output' on test!
            var minKey = results.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Console.WriteLine("Minimum Value: Key: " + minKey.ToString() + ", Value: " + results[minKey]);
            foreach (var item in results)
            {
                Console.WriteLine("Test: " + item.Key + ", result: " + item.Value);
            }
            return results;
        }

        private double CalculateResidual(Vector<double> actualXs, Vector<double> actualYs, Func<double, double> transform)
        {

            Matrix<double> m = Matrix<double>.Build.Dense(actualXs.Count, 2, 0.0);
            for (int i = 0; i < m.RowCount; i++)
            {
                m[i, 0] = 1.0;
                m[i, 1] = transform((double)actualXs[i]);
            }
            Vector<double> betas = CalculateBetas(m, actualYs);
            Vector<double> estimatedYs = CalculateEstimatedYs(m, betas);
            return CalculatateSumOfResidualsSquared(actualYs, estimatedYs);

        }
        private double CalculateLinearResidual(Vector<double> actualXs, Vector<double> actualYs)
        {
            Matrix<double> m = Matrix<double>.Build.Dense(actualXs.Count, 2, 0.0);
            for (int i = 0; i < m.RowCount; i++)
            {
                m[i, 0] = 1.0;
                m[i, 1] = (double)actualXs[i];
            }
            Vector<double> betas = CalculateBetas(m, actualYs);
            Vector<double> estimatedYs = CalculateEstimatedYs(m, betas);
            return CalculatateSumOfResidualsSquared(actualYs, estimatedYs);
        }
        private Vector<double> CalculateBetas(Matrix<double> m, Vector<double> y)
        {
            return (m.Transpose() * m).Inverse() * m.Transpose() * y;
        }

        private Vector<double> CalculateEstimatedYs(Matrix<double> x, Vector<double> beta)
        {
            return x * beta;
        }

        private double CalculatateSumOfResidualsSquared(Vector<double> yReal, Vector<double> yEstimated)
        {
            return ((yReal - yEstimated).PointwisePower(2)).Sum();
        }


    }
}
