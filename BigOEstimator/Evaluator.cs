using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            
        }

        public  Dictionary<FunctionEnum, double> Evaluate(Func<uint,uint> algorithm, IList<double> suggestedList)
        {
            try
            {
                // See for example: https://stackoverflow.com/questions/3836584/lowering-priority-of-task-factory-startnew-thread
                // for a dicussion of changing thread priority
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Dictionary<FunctionEnum, double> results = new Dictionary<FunctionEnum, double>();
                Vector<double> answer = Vector<double>.Build.Dense(suggestedList.Count(), 0.0);
                for (int i = 0; i < suggestedList.Count(); i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var result =  algorithm((uint)suggestedList[i]);
                    stopwatch.Stop();
                    answer[i] = stopwatch.ElapsedTicks;

                    Console.WriteLine($"Answer for index {suggestedList[i]} is {answer[i]}");
                }

                // try "cleaning" data
                Tuple<IList<double>, Vector<double>> cleanedData = CleanData(suggestedList, answer);
                // linear case - N
                results[FunctionEnum.N] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => d);
                // quadratic case - NSquared
                results[FunctionEnum.NSquared] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d));
                // cubic case - NCubed
                results[FunctionEnum.NCubed] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d * d));
                // NLogN case - NLogN
                results[FunctionEnum.NLogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * Math.Log(d)));
                // LogN case - LogN
                results[FunctionEnum.LogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (Math.Log(d)));

                // following few lines are useful for unit tests. You get this by hitting 'Output' on test!
                var minKey = results.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                Console.WriteLine("Minimum Value: Key: " + minKey.ToString() + ", Value: " + results[minKey]);
                foreach (var item in results)
                {
                    Console.WriteLine("Test: " + item.Key + ", result: " + item.Value);
                }
                return results;
            }
            finally
            {
                // Restore the thread default priority.
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
            }
        }

        public async Task<Dictionary<FunctionEnum, double>> EvaluateAsync(Func<uint, Task<uint>> algorithm, IList<double> suggestedList)
        {
            try
            {
                // See for example: https://stackoverflow.com/questions/3836584/lowering-priority-of-task-factory-startnew-thread
                // for a dicussion of changing thread priority
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Dictionary<FunctionEnum, double> results = new Dictionary<FunctionEnum, double>();
                Vector<double> answer = Vector<double>.Build.Dense(suggestedList.Count(), 0.0);
                for (int i = 0; i < suggestedList.Count(); i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var result = await algorithm((uint)suggestedList[i]);
                    stopwatch.Stop();
                    answer[i] = stopwatch.ElapsedTicks;

                    Console.WriteLine($"Answer for index {suggestedList[i]} is {answer[i]}");
                }

                // try "cleaning" data
                Tuple<IList<double>, Vector<double>> cleanedData = CleanData(suggestedList, answer);
                // linear case - N
                results[FunctionEnum.N] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => d);
                // quadratic case - NSquared
                results[FunctionEnum.NSquared] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d));
                // cubic case - NCubed
                results[FunctionEnum.NCubed] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d * d));
                // NLogN case - NLogN
                results[FunctionEnum.NLogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * Math.Log(d)));
                // LogN case - LogN
                results[FunctionEnum.LogN] = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (Math.Log(d)));

                // following few lines are useful for unit tests. You get this by hitting 'Output' on test!
                var minKey = results.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                Console.WriteLine("Minimum Value: Key: " + minKey.ToString() + ", Value: " + results[minKey]);
                foreach (var item in results)
                {
                    Console.WriteLine("Test: " + item.Key + ", result: " + item.Value);
                }
                return results;
            }
            finally
            {
                // Restore the thread default priority.
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
            }
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

        Tuple<IList<double> , Vector<double> > CleanData(IList<double> lst, Vector<double> answers)
        {
            // remove the first few entries which seems to be often very out of whack!
            //lst.RemoveAt(0);
            var newList = lst.Skip(2).ToList();
            var newAnswers = answers.SubVector(2, answers.Count - 2);

            // try to remove any abnormally large items.
            for (int i = 0; i < 10; i ++)
            {
                int maxIndex = newAnswers.MaximumIndex();
                if (maxIndex < newAnswers.Count - 3)
                {
                    newAnswers[maxIndex] = (newAnswers[maxIndex - 1] + newAnswers[maxIndex + 1]) / 2.0;
                }
                else
                {
                    break;
                }
            }
            
            return new Tuple<IList<double>, Vector<double>>(newList, newAnswers);
        }
    }
}
