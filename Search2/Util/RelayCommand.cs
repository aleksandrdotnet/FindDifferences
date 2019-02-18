using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Search2.Util
{
    public class RelayCommand : ICommand
    {
        #region Fields
        private readonly Action<object> _action;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        public string DisplayText { get; private set; }
        #endregion

        #region Constructors
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Action<object> execute, Action<object> action)
            : this(execute, null, action)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute, Action<object> action)
            : this(execute, canExecute, "", action)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute, string displayText, Action<object> action)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            DisplayText = displayText;
            _action = action;
        }
        #endregion

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}
