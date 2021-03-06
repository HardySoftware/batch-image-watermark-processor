﻿using HardySoft.UI.BatchImageProcessor.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model;
using System.Collections.Generic;

namespace HardySoft.UI.BatchImageProcessor.UnitTest
{
    
    
    /// <summary>
    ///This is assembly test class for UtilitiesTest and is intended
    ///to contain all UtilitiesTest Unit Tests
    ///</summary>
	[TestClass()]
	public class UtilitiesTest {


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
		//Use ClassCleanup to run code after all tests in assembly class have run
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


		/// <summary>
		///A test for GetExifContainer
		///</summary>
		[TestMethod()]
		public void GetExifContainerTest() {
			bool includeEnumValueTranslation = true;
			List<ExifContainerItem> expected = null;
			List<ExifContainerItem> actual = Utilities.GetExifContainer(includeEnumValueTranslation);
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod()]
		public void ParseResource_Without_Parameter() {
			string input = "ResourceKey";
			string expected = input;
			string actual = Utilities.ParseResource(input);
			Assert.AreEqual(expected, actual);
		}
	}
}
