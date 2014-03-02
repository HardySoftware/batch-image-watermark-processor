using HardySoft.CC.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HardySoft.UI.BatchImageProcessor.UnitTest {
	[TestClass()]
	public class GpsLocationTest {
		[TestMethod()]
		public void EnterDMS() {
			GeographicCoordinate location = new GeographicCoordinate(79, 58, 56, CoordinateDirection.West);

			Assert.AreEqual(location.CoordinateType, CoordinateType.Longitude);
		}
	}
}
