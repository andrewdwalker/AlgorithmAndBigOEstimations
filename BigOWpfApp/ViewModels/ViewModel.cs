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
            PlotOriginalData(originalPoints, results[FunctionEnum.N].RealYs);
            PlotGeneratedData(originalPoints, results);
        }

        private void PlotOriginalData(List<ulong> xs, List<double> ys)
        {
            for (int i = 0; i < xs.Count; i++)
            {
                ValuesRealData.Add(new ObservablePoint(xs[i], ys[i]));
            }
        }

        private void PlotGeneratedData(List<ulong> xs, Dictionary<FunctionEnum, ResultsForExport> results)
        {
            ChartValues<ObservablePoint> linearData = new ChartValues<ObservablePoint>();
            for (int i = 0; i < xs.Count; i++)
            {
                linearData.Add(new ObservablePoint(xs[i], xs[i] * results[FunctionEnum.N].Slope + results[FunctionEnum.N].SlopeIntercept));
            }

            SeriesCollection.Add(new LineSeries
            {
                Title = "Linear Fit",
                Values = linearData,
                LineSmoothness = 0,
                PointForeground = Brushes.Gray
            });
        }
        #endregion
    }
}
