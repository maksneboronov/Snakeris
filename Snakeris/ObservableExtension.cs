using System;
using System.Collections.ObjectModel;

namespace Snakeris
{
	public static class ObservableExtension
	{
		public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			foreach (var item in collection)
			{
				action(item);
			}
		}
	}
}
