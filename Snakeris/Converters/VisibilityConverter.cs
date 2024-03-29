﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Snakeris
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool) value ? Visibility.Collapsed : Visibility.Visible;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Int32.Parse((string)parameter);
		}
	}
}
