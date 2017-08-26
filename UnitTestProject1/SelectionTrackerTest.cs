using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RotatePictures.Utilities;

namespace UnitTestRotatePictures
{
	[TestClass]
	public class SelectionTrackerTest
	{
		[TestInitialize]
		public void InitializeTest() => SelectionTracker.Inst.ClearTracker();

		[TestMethod]
		public void AppendTest()
		{
			// Arrange
			var expected = 4;

			// Act
			SelectionTracker.Inst.Append("A1");
			SelectionTracker.Inst.Append("B2");
			SelectionTracker.Inst.Append("C3");
			SelectionTracker.Inst.Append("D4");

			// Assert
			Assert.AreEqual(expected, SelectionTracker.Inst.Count);
		}

		[TestMethod]
		public void Append5Prev3Test()
		{
			// Arrange
			var expected = 5;
			var expectedP1 = "D4";
			var expectedP2 = "C3";
			var expectedP3 = "B2";

			// Act
			SelectionTracker.Inst.Append("A1");
			SelectionTracker.Inst.Append("B2");
			SelectionTracker.Inst.Append("C3");
			SelectionTracker.Inst.Append("D4");
			SelectionTracker.Inst.Append("E5");
			var p1 = SelectionTracker.Inst.Prev();
			var p2 = SelectionTracker.Inst.Prev();
			var p3 = SelectionTracker.Inst.Prev();

			// Assert
			Assert.AreEqual(expected, SelectionTracker.Inst.Count);
			Assert.AreEqual(expectedP1, p1);
			Assert.AreEqual(expectedP2, p2);
			Assert.AreEqual(expectedP3, p3);
		}

		[TestMethod]
		public void Append6Prev3Add1Test()
		{
			// Arrange
			var expected = 4;
			var expectedP1 = "E5";
			var expectedP2 = "D4";
			var expectedP3 = "C3";

			// Act
			SelectionTracker.Inst.Append("A1");
			SelectionTracker.Inst.Append("B2");
			SelectionTracker.Inst.Append("C3");
			SelectionTracker.Inst.Append("D4");
			SelectionTracker.Inst.Append("E5");
			SelectionTracker.Inst.Append("F6");
			var p1 = SelectionTracker.Inst.Prev();
			var p2 = SelectionTracker.Inst.Prev();
			var p3 = SelectionTracker.Inst.Prev();
			SelectionTracker.Inst.Add("G7");

			// Assert
			Assert.AreEqual(expected, SelectionTracker.Inst.Count);
			Assert.AreEqual(expectedP1, p1);
			Assert.AreEqual(expectedP2, p2);
			Assert.AreEqual(expectedP3, p3);
		}
	}
}
