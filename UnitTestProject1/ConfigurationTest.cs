using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RotatePictures.Utilities;


namespace UnitTestProject1
{
	[TestClass]
	public class ConfigurationTest
	{
		[TestMethod]
		public void InitialFoldersTest()
		{
			// Arrange
			string[] dirs;

			// Act
			dirs = ConfigValue.Inst.InitialPictureDirectories();

			// Assert
			//<add key="Initial Folders" value="M:\Pictures;G:\Pictures;\\xyz\Pic\Pictures\Pics"/>
			Assert.AreEqual(3, dirs.Length);
			Assert.AreEqual(@"M:\Pictures", dirs[0]);
			Assert.AreEqual(@"G:\Pictures", dirs[1]);
			Assert.AreEqual(@"\\xyz\Pic\Pictures\Pics", dirs[2]);
		}

		[TestMethod]
		public void MaxPictureTrackerDepthTest()
		{
			// Arrange
			int depth;

			// Act
			depth = ConfigValue.Inst.MaxPictureTrackerDepth();

			// Assert
			//<add key = "Max picture tracker depth" value="9999"/>
			Assert.AreEqual(9999, depth);
		}

		[TestMethod]
		public void ExtensionsToConsiderTest()
		{
			// Arrange
			List<string> exts;
			//<add key = "File extentions to consider" value=".jpg;.png;.bmp;.avi;.jpeg;.Peggy;.Ben"/>
			var expected = new List<string> { ".jpg", ".png", ".bmp", ".avi", ".jpeg", ".Peggy", ".Ben" };

			// Act
			exts = ConfigValue.Inst.FileExtensionsToConsider();

			// Assert
			Assert.AreEqual(expected.Count, exts.Count);
			for (var i = 0; i < exts.Count; ++i)
				Assert.AreEqual(expected[i], exts[i]);
		}

		[TestMethod]
		public void ImageStrechValueTest()
		{
			// Arrange
			string actual;
			//<add key = "Image stretch" value="fILL"/>
			var expected = "Fill";

			// Act
			actual = ConfigValue.Inst.ImageStretch();

			// Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
