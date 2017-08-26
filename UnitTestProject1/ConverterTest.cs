using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RotatePictures.Converters;


namespace UnitTestRotatePictures
{
	[TestClass]
	public class ConverterTest
	{
		[TestMethod]
		public void IsStillPictureConverterTestSuccess()
		{
			// Arrange
			var converter = new IsStillPictureConverter();
			const bool expected = true;

			// Act
			var convert = converter.Convert(@"K:\abc\def\ghi\jkl\File.bmp", typeof(bool), null, CultureInfo.CurrentCulture);

			// Assert
			Assert.AreEqual(expected, convert);
		}

		[TestMethod]
		public void IsStillPictureConverterTestFailure()
		{
			// Arrange
			var converter = new IsStillPictureConverter();
			const bool expected = false;

			// Act
			var convert = converter.Convert(@"K:\abc\def\ghi\jkl\File.jkl", typeof(bool), null, CultureInfo.CurrentCulture);

			// Assert
			Assert.AreEqual(expected, convert);
		}

		[TestMethod]
		public void IsMotionPictureConverterTestSuccess()
		{
			// Arrange
			var converter = new IsMotionPictureConverter();
			const bool expected = true;

			// Act
			var convert = converter.Convert(@"K:\abc\def\ghi\jkl\File.ben", typeof(bool), null, CultureInfo.CurrentCulture);

			// Assert
			Assert.AreEqual(expected, convert);
		}

		[TestMethod]
		public void IsMotionPictureConverterTestFailure()
		{
			// Arrange
			var converter = new IsMotionPictureConverter();
			const bool expected = false;

			// Act
			var convert = converter.Convert(@"K:\abc\def\ghi\jkl\File.dll", typeof(bool), null, CultureInfo.CurrentCulture);

			// Assert
			Assert.AreEqual(expected, convert);
		}
	}
}
