using BigOEstimator;
using FibonnacciLibrary;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BigOWpfApp.ViewModels
{
    public enum AlgorithmChoices
    {
        Fibonacci_Faster,
        Fibonacci_NaiveRecursion
    }
    public class ViewModel : ViewModelBase
    {
        #region fields
        private ICommand _runAlgorithmCommand;
        private AlgorithmChoices _selectedAlgorithm;
        private Evaluator _evaluator = new Evaluator();
        private List<ulong> _quadraticListSuggestions = new List<ulong>();
        private List<ulong> _naiveFibSuggestions = new List<ulong>();
        private List<ulong> _efficientFibSuggestions = new List<ulong>();
        
        #endregion
        public ViewModel ()
        {
            Algorithms = new ObservableCollection<AlgorithmChoices>();
            Algorithms.Add(AlgorithmChoices.Fibonacci_NaiveRecursion);
            Algorithms.Add(AlgorithmChoices.Fibonacci_Faster);

            for (ulong i = 5000; i < 10000; i = i + 100)
            {
                _quadraticListSuggestions.Add(i);
            }

            for (ulong i = 25; i < 40; i = i + 1)
            {
                _naiveFibSuggestions.Add(i);
            }

            for (ulong i = 1500000; i < 10000000; i = i + 100000)
            {
                _efficientFibSuggestions.Add(i);
            }

            ValuesRealData = new ChartValues<ObservablePoint>();
            SeriesCollection = new SeriesCollection();
        }
        #region Properties
        public ObservableCollection<AlgorithmChoices> Algorithms { get; private set; }
        public AlgorithmChoices SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set
            {
                if (value == _selectedAlgorithm) return;
                _selectedAlgorithm = value;
            }
        }

        public SeriesCollection SeriesCollection
        {
            get; set;
        }

        public ChartValues<ObservablePoint> ValuesRealData { get; set; }

        
        #endregion

        #region CommandHandlers
        public ICommand RunAlgorithmCommand
        {
            get
            {
                if (_runAlgorithmCommand == null)
                {
                    _runAlgorithmCommand = new RelayCommand(
                   param => RunAlgorithm()
                    );
                }
                return _runAlgorithmCommand;
            }


        }
        #endregion

        #region Methods
        private void RunAlgorithm()
        {
            Dictionary<FunctionEnum, ResultsForExport> results = null;
            switch (SelectedAlgorithm)
            {
                case AlgorithmChoices.Fibonacci_Faster:
                    results = _evaluator.Evaluate(FibonacciCalculations.JustOneFibSimpleLoop, _efficientFibSuggestions);
                    PlotResults(results, _efficientFibSuggestions);
                    break;
                case AlgorithmChoices.Fibonacci_NaiveRecursion:
                    results = _evaluator.Evaluate(FibonacciCalculations.JustOneFib, _naiveFibSuggestions);
                    PlotResults(results, _naiveFibSuggestions);
                    break;
            }
        }

        private void PlotResults( Dictionary<FunctionEnum, ResultsForExport> results, List<ulong> originalPoints)
        {
            SeriesCollection.Clear();
            PlotOriginalData(originalPoints, results[FunctionEnum.N].RealYs);
            PlotGeneratedData(originalPoints, results[FunctionEnum.N].Slope, results[FunctionEnum.N].SlopeIntercept,  Brushes.Gray,FunctionEnum.N);
            PlotGeneratedData(originalPoints, results[FunctionEnum.TwoToTheN].Slope, results[FunctionEnum.TwoToTheN].SlopeIntercept, Brushes.Green, FunctionEnum.TwoToTheN);
           PlotGeneratedData(originalPoints, results[FunctionEnum.NSquared].Slope, results[FunctionEnum.NSquared].SlopeIntercept, Brushes.Green, FunctionEnum.NSquared);
           PlotGeneratedData(originalPoints, results[FunctionEnum.LogN].Slope, results[FunctionEnum.LogN].SlopeIntercept, Brushes.Green, FunctionEnum.LogN);
        }

        private void PlotOriginalData(List<ulong> xs, List<double> ys)
        {
            ChartValues<ObservablePoint> linearData = new ChartValues<ObservablePoint>();
            for (int i = 0; i < xs.Count; i++)
            {
                ValuesRealData.Add(new ObservablePoint(xs[i], ys[i]));
                linearData.Add(new ObservablePoint(xs[i], ys[i]));
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = "Raw Data",
                Values = linearData,
                LineSmoothness = 0,
                PointForeground = Brushes.Blue,

            });
        }

        private void PlotGeneratedData(List<ulong> xs, double slope, double intercept,  Brush brush, FunctionEnum fitType)
        {
            if (double.IsNaN(slope) || double.IsNaN(intercept))
            {
                Console.WriteLine($"Bad slope {slope} and/or intercept {intercept} ");
                return;
            }
            List<double> modifiedXs = new List<double>();
            string title = "Unknown";
            switch(fitType)
            {
                case FunctionEnum.N:
                    title = "Linear Fit";
                    modifiedXs = ModifyData(xs, p => p);
                    break;
                case FunctionEnum.NSquared:
                    title = "Quadratic Fit";
                    modifiedXs = ModifyData(xs, p => p * p);
                    break;
                case FunctionEnum.TwoToTheN:
                    title = "2^N";
                    modifiedXs = ModifyData(xs, p => Math.Pow(2, p));
                    break;
                case FunctionEnum.NCubed:
                    title = "Cubit Fit";
                    modifiedXs = ModifyData(xs, p => p * p * p);
                    break;
                case FunctionEnum.LogN:
                    title = "LogN Fit";
                    modifiedXs = ModifyData(xs, p => Math.Log(p));
                    break;
            }
            ChartValues<ObservablePoint> linearData = new ChartValues<ObservablePoint>();
            for (int i = 0; i < modifiedXs.Count; i++)
            {
                linearData.Add(new ObservablePoint(xs[i], modifiedXs[i] * slope + intercept));
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = title,
                Values = linearData,
                LineSmoothness = 0,
                PointForeground = brush
                
            });
        }

        private List<double> ModifyData(List<ulong> xs, Func<double, double> transform)
        {
            List<double> modifiedXs = new List<double>();
            for (int i=0; i< xs.Count; i++)
            {
                modifiedXs.Add(transform(xs[i]));
            }

            return modifiedXs;
        }
        #endregion
    }
}
