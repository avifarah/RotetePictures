using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RotatePictures.Utilities;


namespace RotatePictures.Converters
{
	public class IsStillPictureConverter : IValueConverter
	{
		private static List<string> _stillPictures;

		public IsStillPictureConverter()
		{
			if (_stillPictures == null) _stillPictures = ConfigValue.Inst.StillPictureExtensions();
		}

		#region Implementation of IValueConverter

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var fileNm = value as string;
			if (fileNm == null) return false;
			return _stillPictures.Any(s => string.Compare(new FileInfo(fileNm).Extension, s, StringComparison.OrdinalIgnoreCase) == 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

		#endregion
	}
}
