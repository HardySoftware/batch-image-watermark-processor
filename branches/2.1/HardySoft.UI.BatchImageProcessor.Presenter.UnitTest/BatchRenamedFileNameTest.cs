using HardySoft.UI.BatchImageProcessor.Presenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model;
using System.Collections.Generic;

namespace HardySoft.UI.BatchImageProcessor.Presenter.UnitTest {


	/// <summary>
	///This is a test class for BatchRenamedFileNameTest and is intended
	///to contain all BatchRenamedFileNameTest Unit Tests
	///</summary>
	[TestClass()]
	public class BatchRenamedFileNameTest {


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
		public void GetFileNameTest() {
			List<string> files = new List<string>();
			for (uint i = 0; i < 10; i++) {
				BatchRenamedFileName target = new BatchRenamedFileName(i);
				string sourceFile = @"C\photos\folder 1\123.jpg";

				ProjectSetting ps = new ProjectSetting();
				ps.OutputDirectory = @"C:\Out";
				ps.RenamingSetting.OutputFileNamePrefix = "travel_";
				ps.RenamingSetting.OutputFileNameSuffix = "_city";
				ps.RenamingSetting.StartNumber = 0;
				ps.RenamingSetting.FileNameCase = OutputFileNameCase.None;
				string outputFileName = target.GetFileName(sourceFile, ps);
				if (!files.Contains(outputFileName)) {
					files.Add(outputFileName);
				}
			}

			Assert.AreEqual(files.Count, 10);
		}

		[TestMethod()]
		public void GetFileNameTest1() {
			BatchRenamedFileName target = new BatchRenamedFileName(10);
			string sourceFile = @"C\photos\folder 1\123.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Out";
			ps.RenamingSetting.OutputFileNamePrefix = "travel_";
			ps.RenamingSetting.OutputFileNameSuffix = "_city";
			ps.RenamingSetting.StartNumber = 0;
			ps.RenamingSetting.FileNameCase = OutputFileNameCase.None;

			string expected = @"C:\Out\travel_010_city.jpg";
			string actual = target.GetFileName(sourceFile, ps);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetFileNameTest2() {
			BatchRenamedFileName target = new BatchRenamedFileName(10);
			string sourceFile = @"C\photos\folder 1\123.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Out";
			ps.RenamingSetting.OutputFileNamePrefix = "travel_";
			ps.RenamingSetting.OutputFileNameSuffix = "_city";
			ps.RenamingSetting.StartNumber = 5;
			ps.RenamingSetting.FileNameCase = OutputFileNameCase.None;

			string expected = @"C:\Out\travel_015_city.jpg";
			string actual = target.GetFileName(sourceFile, ps);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetFileNameTest3() {
			BatchRenamedFileName target = new BatchRenamedFileName(10);
			string sourceFile = @"C\photos\folder 1\123.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Out";
			ps.RenamingSetting.OutputFileNamePrefix = "travel_";
			ps.RenamingSetting.OutputFileNameSuffix = "_city";
			ps.RenamingSetting.StartNumber = 5;
			ps.RenamingSetting.NumberPadding = 6;
			ps.RenamingSetting.FileNameCase = OutputFileNameCase.None;

			string expected = @"C:\Out\travel_000015_city.jpg";
			string actual = target.GetFileName(sourceFile, ps);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetFileNameTest4() {
			BatchRenamedFileName target = new BatchRenamedFileName(10);
			string sourceFile = @"C\photos\folder 1\DC001.jpg";

			ProjectSetting ps = new ProjectSetting();
			ps.OutputDirectory = @"C:\Out";
			ps.RenamingSetting.OutputFileNamePrefix = string.Empty;
			ps.RenamingSetting.OutputFileNameSuffix = string.Empty;
			ps.RenamingSetting.StartNumber = 5;
			ps.RenamingSetting.NumberPadding = 6;
			ps.RenamingSetting.FileNameCase = OutputFileNameCase.None;

			string expected = @"C:\Out\DC001_000015.jpg";
			string actual = target.GetFileName(sourceFile, ps);
			Assert.AreEqual(expected, actual);
		}
	}
}
