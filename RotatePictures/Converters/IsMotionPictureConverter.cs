using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using RotatePictures.Utilities;


namespace RotatePictures.Converters
{
	public class IsMotionPictureConverter : IValueConverter
	{
		private static List<string> _motionPictures;

		public IsMotionPictureConverter()
		{
			if (_motionPictures == null) _motionPictures = ConfigValue.Inst.MotionPictures();
		}

		#region Implementation of IValueConverter

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var fileNm = value as string;
			if (fileNm == null) return false;
			return _motionPictures.Any(s => string.Compare(new FileInfo(fileNm).Extension, s, StringComparison.OrdinalIgnoreCase) == 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

		#endregion
	}
}
