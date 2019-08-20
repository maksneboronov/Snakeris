using System;
using System.Windows.Input;

namespace MVVM.Core.RelayCommand
{
	public class RelayCommandFactory : ICommandFactory
	{
		public ICommand CreateCommand(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			return new RelayCommand<object>(o => action());
		}

		public ICommand CreateCommand(Action action, Func<bool> canAction)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			if (canAction == null)
			{
				throw new ArgumentNullException(nameof(canAction));
			}
			return new RelayCommand<object>(o => action(), o => canAction());
		}

		public ICommand CreateCommand<T>(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			return new RelayCommand<T>(action);
		}

		public ICommand CreateCommand<T>(Action<T> action, Func<T, bool> canAction)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			if (canAction == null)
			{
				throw new ArgumentNullException(nameof(canAction));
			}
			return new RelayCommand<T>(action, canAction);
		}

		public ICommand CreateCommand(Action<object> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			return new RelayCommand<object>(action);
		}

		public ICommand CreateCommand(Action<object> action, Func<object, bool> canAction)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}
			if (canAction == null)
			{
				throw new ArgumentNullException(nameof(canAction));
			}
			return new RelayCommand<object>(action, canAction);
		}
	}
}
