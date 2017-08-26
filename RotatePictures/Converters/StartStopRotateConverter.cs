using System;
using System.Globalization;
using System.Windows.Data;

namespace RotatePictures.Converters
{
	public class StartStopRotateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var running = (bool)value;
			return running ? "Green" : "Red";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}
