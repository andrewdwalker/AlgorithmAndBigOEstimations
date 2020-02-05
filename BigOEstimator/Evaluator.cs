

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

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

    public struct ResultsForExport
    {
        public List<double> RealYs { get; set; }
        public double Slope { get; set; }
        public double SlopeIntercept { get; set; }
        public double SumOfResiduals { get; set; }
    }
    public class Evaluator
    {
        private Dictionary<FunctionEnum, double> _results = new Dictionary<FunctionEnum, double>(); 
        public Evaluator()
        {
            
        }


        public Dictionary<FunctionEnum, ResultsForExport> Evaluate(Func<ulong, ulong> algorithm, IList<ulong> suggestedList,
            bool cleanData = false)
        {
            try
            {
                // See for example: https://stackoverflow.com/questions/3836584/lowering-priority-of-task-factory-startnew-thread
                // for a dicussion of changing thread priority
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Tuple<double, Tuple<double, double>> results; // = new Dictionary<FunctionEnum, Tuple<double, Tuple<double, double>>>();
                Dictionary<FunctionEnum, ResultsForExport> grandResults = new Dictionary<FunctionEnum, ResultsForExport>();
                Vector<double> answer = Vector<double>.Build.Dense(suggestedList.Count(), 0.0);
                for (int i = 0; i < suggestedList.Count(); i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var result =  algorithm((ulong) suggestedList[i] );
                    stopwatch.Stop();
                    answer[i] = stopwatch.ElapsedMilliseconds;

                    Console.WriteLine($"Answer for index {suggestedList[i]} is {answer[i]}");
                }

                // MathNet only supports buiding vectors out of doubles, single, complex or complex32. Sigh...
                IList<double> suggestedListAsDouble = suggestedList.Select(x => (double)x).ToList();

                Tuple<IList<double>, Vector<double>> cleanedData =
                    new Tuple<IList<double>, Vector<double>>(suggestedListAsDouble, answer);

                if (cleanData)
                // try "cleaning" data
                cleanedData = CleanData(suggestedListAsDouble, answer);
                for (int i = 0; i < cleanedData.Item1.Count; i++)
                {
                    
                    Console.WriteLine($"Cleaned Answer for index {cleanedData.Item1[i]} is {cleanedData.Item2[i]}");
                }
                // linear case - N
                results = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => d);
                grandResults[FunctionEnum.N] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };
                // quadratic case - NSquared
                results = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d));
                grandResults[FunctionEnum.NSquared] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };
                // cubic case - NCubed
                results = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * d * d));
                grandResults[FunctionEnum.NCubed] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };
                // NLogN case - NLogN
                results = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (d * Math.Log(d)));
                grandResults[FunctionEnum.NLogN] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };
                // LogN case - LogN
                results = CalculateResidual(Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (Math.Log(d)));
                grandResults[FunctionEnum.LogN] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };
                // 2^N Case - TwoToTheN
                results = CalculateResidual (Vector<double>.Build.DenseOfEnumerable(cleanedData.Item1), cleanedData.Item2, d => (Math.Pow(2, d)));
                grandResults[FunctionEnum.TwoToTheN] = new ResultsForExport()
                {
                    RealYs = answer.ToList(),
                    SumOfResiduals = results.Item1,
                    SlopeIntercept = results.Item2.Item1,
                    Slope = results.Item2.Item2
                };

                // following few lines are useful for unit tests. You get this by hitting 'Output' on test!
                var minKey = grandResults.Aggregate((l, r) => ((l.Value.SumOfResiduals < r.Value.SumOfResiduals) || double.IsNaN(r.Value.SumOfResiduals)) ? l : r).Key;
                Console.WriteLine("Minimum Value: Key: " + minKey.ToString() + ", Value: " + grandResults[minKey].SumOfResiduals);
                foreach (var item in grandResults)
                {
                    Console.WriteLine($"Test: {item.Key} , squared residuals:  {item.Value.SumOfResiduals}." +
                        $" Constant Term: {item.Value.SlopeIntercept} and slope: {item.Value.Slope}");
                }
                return grandResults;
            }
            finally
            {
                // Restore the thread default priority.
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
            }
        }

        private Tuple<double, Tuple<double, double>> CalculateResidual(Vector<double> actualXs, Vector<double> actualYs, Func<double, double> transform)
        {

            Matrix<double> m = Matrix<double>.Build.Dense(actualXs.Count, 2, 0.0);
            for (int i = 0; i < m.RowCount; i++)
            {
                m[i, 0] = 1.0;
                m[i, 1] = transform(actualXs[i]);
            } 
            Vector<double> betas = CalculateBetas(m, actualYs); // beta[0] corresponds to b, beta[1] to m !
            Vector<double> estimatedYs = CalculateEstimatedYs(m, betas);
            double sumOfResiduals = CalculatateSumOfResidualsSquared(actualYs, estimatedYs);
            return new Tuple<double, Tuple<double, double>>(sumOfResiduals,
                new Tuple<double, double> (betas[0], betas[1]));

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
            var newList = lst.Skip(10).Take(lst.Count - 20).ToList();
            var newAnswers = answers.SubVector(10, answers.Count - 20);

            List<int> extremeNumbers = new List<int>();
            for (int i = 0; i < newAnswers.Count-5; i++)
            {
                if (newAnswers[i] > newAnswers.SubVector(i+1,5).Average())
                {
                    extremeNumbers.Insert(0, i);
                }
            }
            Console.WriteLine($"Number of extremeNumbers: {extremeNumbers.Count}");
            for (int i = 0; i < extremeNumbers.Count; i++)
            {
                newList.RemoveAt(extremeNumbers[i]);
                newAnswers = DeleteAt<double>(newAnswers, extremeNumbers[i]);
            }
            ////try to remove any abnormally large items.
            //for (int i = 0; i < 10; i++)
            //{

            //    int maxIndex = newAnswers.SubVector(0, newAnswers.Count - 10).MaximumIndex();
            //    if (newAnswers[maxIndex] > newAnswers.SubVector(maxIndex + 1, 5).Average())
            //    {
            //        newList.RemoveAt(maxIndex);
            //        newAnswers = DeleteAt<double>(newAnswers, maxIndex);
            //    }

            //    //if (maxIndex < newAnswers.Count - 3)
            //    //{
            //    //    newAnswers[maxIndex] = (newAnswers[maxIndex - 1] + newAnswers[maxIndex + 1]) / 2.0;
            //    //}
            //    //else
            //    //{
            //    //    break;
            //    //}
            //}

            return new Tuple<IList<double>, Vector<double>>(newList, newAnswers);
        }

        // See: https://stackoverflow.com/questions/19157002/how-to-add-a-new-element-into-a-densevector
        Vector<T> InsertAt<T>(Vector<T> v, int i, T value) where T : struct, IEquatable<T>, IFormattable
        {
            var res = Vector<T>.Build.Dense(v.Count + 1);
            if (i > 0) v.Storage.CopySubVectorTo(res.Storage, 0, 0, i);
            if (i < v.Count) v.Storage.CopySubVectorTo(res.Storage, i, i + 1, v.Count - i);
            res.At(i, value);
            return res;
        }

        Vector<T> DeleteAt<T>(Vector<T> v, int i) where T : struct, IEquatable<T>, IFormattable
        {
            var res = Vector<T>.Build.Dense(v.Count -1 );
            if (i > 0) v.Storage.CopySubVectorTo(res.Storage, 0, 0, i);
            if (i < v.Count) v.Storage.CopySubVectorTo(res.Storage, i+1, i, v.Count - i-1);
            
            return res;
        }
    }
}
