using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RotatePictures
{
	public class ColorSwapper : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) { return DependencyProperty.UnsetValue; }

			Color srcColor = (value as SolidColorBrush).Color;
			if (srcColor == Colors.Black)
			{
				return new SolidColorBrush(Colors.White);
			}
			else
			{
				return new SolidColorBrush(Colors.Black);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}
