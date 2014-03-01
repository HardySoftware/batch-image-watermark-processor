using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model.Exif;

namespace HardySoft.UI.BatchImageProcessor.UnitTest {
	[TestClass()]
	public class GpsLocationTest {
		[TestMethod()]
		public void EnterDMS() {
			GpsLocation location = new GpsLocation(79, 58, 56, CoordinateDirection.West);

			Assert.AreEqual(location.CoordinateType, CoordinateType.Longitude);
		}
	}
}
