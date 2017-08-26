using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace RotatePictures.Utilities
{
	public static class StringUtil
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly List<string> _trues = new List<string> { "True", "T", "OK", "K", "Yes", "Y", "Positive", "P", "+", "1"};
		private static readonly List<string> _falses = new List<string> { "False", "F", "No", "N", "Negative", "-", "0" };

		public static bool IsTrue(this string text) => _trues.Any(t => string.Compare(text, t, StringComparison.OrdinalIgnoreCase) == 0);

		public static bool IsFalse(this string text) => _falses.Any(f => string.Compare(text, f, StringComparison.OrdinalIgnoreCase) == 0);

		public static bool IsFloat(this string text)
		{
			var regex = new Regex(@"[0-9\.\,]+");
			return regex.IsMatch(text);
		}
	}
}
