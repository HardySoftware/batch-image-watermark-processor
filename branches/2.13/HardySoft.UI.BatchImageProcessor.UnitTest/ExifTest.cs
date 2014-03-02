using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardySoft.UI.BatchImageProcessor.Model.Exif;
using HardySoft.CC.Mathematics;

namespace HardySoft.UI.BatchImageProcessor.UnitTest {
	[TestClass()]
	public class ExifTest {
		private string testImageFile = @"D:\Temp\ConsoleApplication7\Images\WP_000156.JPG";
		//private string testImageFile = @"D:\Temp\ConsoleApplication7\Images\P1040092.JPG";

		[TestMethod()]
		public void ExifRead_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
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
			var a18 = meta.LensAperture;
			var a19 = meta.LightSource;
			var a21 = meta.Longitude;
			var a22 = meta.MeteringMode;
			var a23 = meta.VerticalResolution;
			var a24 = meta.Width;
		}

		[TestMethod()]
		public void ExifWrite_Altitude_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.Altitude = -1454.67f;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_ColorRepresentation_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.ColorRepresentation = ColorRepresentation.sRGB;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_CreationSoftware_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.CreationSoftware = "Sea Turtle Batch Image Processor";
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_DateImageTaken_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.DateImageTaken = DateTime.Now;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_EquipmentManufacturer_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.EquipmentManufacturer = "My PC";
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_ExifVersion_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.ExifVersion = "0221";
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_ExposureCompensation_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.ExposureCompensation = new Fraction(2, 10);
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_ExposureMode_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.ExposureMode = ExposureMode.PortraitMode;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_ExposureTime_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.ExposureTime = new Fraction(17, 95);
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_FlashMode_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.FlashMode = FlashMode.FlashAutoRedEye;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_FocalLength_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.FocalLength = new Fraction(12.6);
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_Height_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.Height = 1234;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_HorizontalResolution_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.HorizontalResolution = 1234;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_IsoSpeed_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.IsoSpeed = 320;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_Latitude_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.Latitude = new GeographicCoordinate(45, 26, 37.445f, CoordinateDirection.South);
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_LensAperture_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.LensAperture = 11.2;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_LightSource_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.LightSource = LightSourceMode.CloudyWeather;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_Longitude_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.Longitude = new GeographicCoordinate(67, 19, 45, CoordinateDirection.East);
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_MeteringMode_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.MeteringMode = MeteringMode.Spot;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_VerticalResolution_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.VerticalResolution = 2345;
			meta.SaveExif();
		}

		[TestMethod()]
		public void ExifWrite_Width_Integration_Test() {
			ExifMetadata meta = new ExifMetadata(new Uri(this.testImageFile));
			meta.Width = 1122;
			meta.SaveExif();
		}
	}
}
