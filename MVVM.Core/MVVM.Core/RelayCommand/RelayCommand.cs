using System;
using System.Windows.Input;

namespace MVVM.Core.RelayCommand
{
	public class RelayCommand<T> : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		private readonly Func<T, bool> _canExecute;
		private readonly Action<T> _execute;

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		bool ICommand.CanExecute(object parameter)
			=> this.CanExecute((T)parameter);

		void ICommand.Execute(object parameter) 
			=> this.Execute((T)parameter);

		public bool CanExecute(T parameter)
			=> _canExecute == null || _canExecute((T)parameter);

		public void Execute(T parameter) 
			=> _execute(parameter);
	}
}
