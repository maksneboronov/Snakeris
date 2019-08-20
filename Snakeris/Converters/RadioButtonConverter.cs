using System;
using System.Globalization;
using System.Windows.Data;

namespace Snakeris
{
	public class RadioButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value).ToString() == (string)parameter; 
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Int32.Parse((string)parameter);
		}
	}
}
