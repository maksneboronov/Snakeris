using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MVVM.Core.NotifyProperyChanged
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RaisableAttribute : Attribute
	{
		public RaisableAttribute(string dependProp) => DependProperty = dependProp;

		public string DependProperty { get; set; }
	}

	public class NotifyPropertyChanged : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void UpdateProperty<T>(T value, ref T source, [CallerMemberName] string name = "")
		{
			if (EqualityComparer<T>.Default.Equals(value, source))
			{
				return;
			}

			source = value;
			if (!String.IsNullOrWhiteSpace(name))
			{
				RaisePropertyChanged(name);
			}
		}

		public NotifyPropertyChanged()
		{
			_dict = this
				.GetType()
				.GetProperties()
				.ToDictionary(
					i => i.Name,
					i => i.GetCustomAttributes(typeof(RaisableAttribute), false)
							.Select(j => (RaisableAttribute)j)
							.Select(j => j.DependProperty)
							.ToList()
					);
		}

		protected virtual void RaisePropertyChanged([CallerMemberName] string name = "")
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException(nameof(name));
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
			foreach (var item in _dict[name])
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}


		private readonly Dictionary<string, List<string>> _dict = new Dictionary<string, List<string>>();
	}
}
