using System;
using System.Windows.Input;

namespace MVVM.Core.RelayCommand
{
	public interface ICommandFactory
	{
		ICommand CreateCommand(Action action);
		ICommand CreateCommand(Action action, Func<bool> canAction);
		ICommand CreateCommand<T>(Action<T> action);
		ICommand CreateCommand<T>(Action<T> action, Func<T, bool> canAction);
		ICommand CreateCommand(Action<object> action);
		ICommand CreateCommand(Action<object> action, Func<object, bool> canAction);
	}
}
