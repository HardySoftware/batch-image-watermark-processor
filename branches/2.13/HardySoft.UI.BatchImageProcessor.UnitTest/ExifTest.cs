using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model.Exif;

namespace HardySoft.UI.BatchImageProcessor.UnitTest {
	[TestClass()]
	public class ExifTest {
		[TestMethod()]
		public void ExifIntegrationTest() {
			ExifMetadata meta = new ExifMetadata(new Uri(@"D:\Temp\ConsoleApplication7\Images\WP_000156.JPG"));
			var a1 = meta.Altitude;
			var a2 = meta.CameraModel;
			var a3 = meta.ColorRepresentation;
			var a4 = meta.CreationSoftware;
			var a5 = meta.DateImageTaken;
			var a6 = meta.EquipmentManufacturer;
			var a7 = meta.ExifVersion;
			var a8 = meta.ExposureCompensation;
			var a9 = meta.ExposureMode;
			var a10 = meta.ExposureTime;
			var a11 = meta.FlashMode;
			var a12 = meta.FocalLength;
			var a13 = meta.Height;
			var a14 = meta.HorizontalResolution;
			var a15 = meta.IsoSpeed;
			var a16 = meta.Latitude;
			var a17 = meta.LatitudeRaw;
			var a18 = meta.LensAperture;
			var a19 = meta.LightSource;
			var a20 = meta.Longitude;
			var a21 = meta.LongitudeRaw;
			var a22 = meta.MeteringMode;
			var a23 = meta.VerticalResolution;
			var a24 = meta.Width;
		}
	}
}
