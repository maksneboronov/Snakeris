using System;
using System.Globalization;
using System.Windows.Data;

namespace Snakeris
{
	public class DirectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var direction = (string)parameter;
			return Enum.Parse(typeof(Direction), direction);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}
