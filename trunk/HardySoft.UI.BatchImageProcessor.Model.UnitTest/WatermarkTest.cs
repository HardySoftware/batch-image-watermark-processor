using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using HardySoft.CC;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HardySoft.UI.BatchImageProcessor.Model.UnitTest {
	/// <summary>
	/// Summary description for WatermarkTest
	/// </summary>
	[TestClass]
	public class WatermarkTest {
		public WatermarkTest() {
			//
			// TODO: Add constructor logic here
			//
		}

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
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void Is_Base_Class_Searilzable() {
			//
			// TODO: Add test logic	here
			//

			ProjectSetting ps = new ProjectSetting();
			WatermarkText wt =  new WatermarkText {
			                                      	Text = "Hello world",
			                                      	WatermarkPosition = ContentAlignment.MiddleLeft,
			                                      	WatermarkRotateAngle = 12,
			                                      	WatermarkTextAlignment = StringAlignment.Center
			                                      };
			ps.WatermarkCollection.Add(wt);

			WatermarkImage wi = new WatermarkImage {
				WatermarkImageFile = @"c:\temp\1.jpg",
				WatermarkPosition = ContentAlignment.MiddleCenter
			                                       };

			ps.WatermarkCollection.Add(wi);


			MemoryStream stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, ps);

			stream.Position = 0;

			ProjectSetting ps1 = (ProjectSetting) formatter.Deserialize(stream);
			Assert.AreEqual(ps1.WatermarkCollection.Count, 2);
			//Assert.IsNull(ps1.WatermarkCollection[2]);
			//Assert.IsNull(ps1.WatermarkCollection[3]);
			Assert.IsInstanceOfType(ps1.WatermarkCollection[0], typeof(WatermarkText));
			Assert.IsInstanceOfType(ps1.WatermarkCollection[1], typeof(WatermarkImage));
		}
	}
}
