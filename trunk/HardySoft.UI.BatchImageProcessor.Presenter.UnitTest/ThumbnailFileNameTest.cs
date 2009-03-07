using HardySoft.UI.BatchImageProcessor.Presenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter.UnitTest {
	/// <summary>
	///This is a test class for ThumbnailFileNameTest and is intended
	///to contain all ThumbnailFileNameTest Unit Tests
	///</summary>
	[TestClass()]
	public class ThumbnailFileNameTest {
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
		public void GetFileNameTest1() {
			ThumbnailFileName target = new ThumbnailFileName();

			string sourceFile = @"C:\photos\folder 1\abc.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Temp";
			ps.ThumbnailSetting.GenerateThumbnail = true;
			ps.ThumbnailSetting.ThumbnailFileNamePrefix = "MyPrefix_";
			ps.ThumbnailSetting.ThumbnailFileNameSuffix = "_MySuffix";

			string expected = @"C:\Temp\MyPrefix_abc_MySuffix.jpg";
			string actual;
			actual = target.GetFileName(sourceFile, ps, null);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetFileNameTest2() {
			ThumbnailFileName target = new ThumbnailFileName();

			string sourceFile = @"C:\photos\folder 1\abc.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Temp";
			ps.ThumbnailSetting.GenerateThumbnail = true;
			ps.ThumbnailSetting.ThumbnailFileNamePrefix = string.Empty;
			ps.ThumbnailSetting.ThumbnailFileNameSuffix = string.Empty;

			string expected = @"C:\Temp\abc_thumb.jpg";
			string actual;
			actual = target.GetFileName(sourceFile, ps, null);
			Assert.AreEqual(expected, actual);
		}
	}
}
