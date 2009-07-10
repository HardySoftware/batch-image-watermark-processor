using HardySoft.UI.BatchImageProcessor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.UI.BatchImageProcessor.Model.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for MacroTagValidatorTest and is intended
    ///to contain all MacroTagValidatorTest Unit Tests
    ///</summary>
	[TestClass()]
	public class MacroTagValidatorTest {


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
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest1() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);
			string input = "sajfkh [[klfjks]] jflkj";
			List<string> expected = new List<string>();
			expected.Add("klfjks");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest2() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);

			string input = "sajfkh [[klfjks]] [[jflkj]]";
			List<string> expected = new List<string>();
			expected.Add("klfjks");
			expected.Add("jflkj");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest3() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);

			string input = "sajfkh [[klfjks [[jflkj]]";
			List<string> expected = new List<string>();
			expected.Add("klfjks [[jflkj");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest4() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);

			string input = "sajfkh [[klfjks ]]jflkj]]";
			List<string> expected = new List<string>();
			expected.Add("klfjks");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest5() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);

			string input = "sajfkh [[klfjks ]]jflkj]] safsdf  [[gdsfg]] ds";
			List<string> expected = new List<string>();
			expected.Add("klfjks");
			expected.Add("gdsfg");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void getAllTagsTest6() {
			string[] validTags = new string[2] { "DateTime", "ISO" };
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(validTags);

			string input = "sajfkh [[klfjks [[jflkj]] safsdf  [[gdsfg]] ds";
			List<string> expected = new List<string>();
			expected.Add("klfjks [[jflkj");
			expected.Add("gdsfg");
			string[] actual = target.getAllTags(input);
			Assert.AreEqual(expected.Count, actual.Length);
			for (int i = 0; i < expected.Count; i++) {
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		/// <summary>
		///A test for DoValidate
		///</summary>
		[TestMethod()]
		[DeploymentItem("HardySoft.UI.BatchImageProcessor.Model.dll")]
		public void DoValidateTest() {
			MacroTagValidator_Accessor target = new MacroTagValidator_Accessor(new string[2] { "DateTime", "ISO" });
			string objectToValidate = "sajfkh [[klfjks jflkj]] safsdf  [[ISO]] ds";
			object currentTarget = null;
			string key = string.Empty;
			ValidationResults validationResults = new ValidationResults();
			target.DoValidate(objectToValidate, currentTarget, key, validationResults);
			Assert.AreEqual<int>(validationResults.Count, 1);
		}
	}
}
