using HardySoft.UI.BatchImageProcessor.Presenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Presenter.UnitTest {
	/// <summary>
	///This is a test class for ResizeByHeightTest and is intended
	///to contain all ResizeByHeightTest Unit Tests
	///</summary>
	[TestClass()]
	public class ResizeByHeightTest {
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		[TestMethod()]
		public void CalculateNewSizeTest1() {
			ResizeByHeight target = new ResizeByHeight();
			Size originalSize = new Size(400, 250);
			double newSize = 200;
			Size expected = new Size(320, 200);
			Size actual = target.CalculateNewSize(originalSize, newSize);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void CalculateNewSizeTest2() {
			ResizeByHeight target = new ResizeByHeight();
			Size originalSize = new Size(250, 400);
			double newSize = 200;
			Size expected = new Size(125, 200);
			Size actual = target.CalculateNewSize(originalSize, newSize);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void CalculateNewSizeTest3() {
			ResizeByHeight target = new ResizeByHeight();
			Size originalSize = new Size(400, 300);
			double newSize = 400;
			Size expected = new Size(400, 300);
			Size actual = target.CalculateNewSize(originalSize, newSize);
			Assert.AreEqual(expected, actual);
		}
	}
}