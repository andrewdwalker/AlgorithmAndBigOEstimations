using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigOWpfApp.ViewModels
{
    public enum AlgorithmChoices
    {
        Fibonacci_SimpleLoop,
        Fibonacci_NaiveRecursion
    }
    public class ViewModel : ViewModelBase
    {
        #region fields
        private ICommand _runAlgorithmCommand;
        private AlgorithmChoices _selectedAlgorithm;
        
        #endregion
        public ViewModel ()
        {
            Algorithms = new ObservableCollection<AlgorithmChoices>();
            Algorithms.Add(AlgorithmChoices.Fibonacci_NaiveRecursion);
            Algorithms.Add(AlgorithmChoices.Fibonacci_SimpleLoop);
            
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
            switch (SelectedAlgorithm)
            {
                case AlgorithmChoices.Fibonacci_SimpleLoop:
                    break;
                case AlgorithmChoices.Fibonacci_NaiveRecursion:
                    break;
            }
        }
        #endregion
    }
}
