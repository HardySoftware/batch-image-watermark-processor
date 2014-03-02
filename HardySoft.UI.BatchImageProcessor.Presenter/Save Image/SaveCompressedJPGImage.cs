using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using HardySoft.UI.BatchImageProcessor.Model.Exif;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to save JPG image with certain compression ratio.
	/// </summary>
	class SaveCompressedJPGImage : ISaveImage {
		private long compressionRatio;
		private ExifMetadata exif;

		public SaveCompressedJPGImage(long compressionRatio) {
			this.compressionRatio = compressionRatio;
		}

		public ExifMetadata Exif {
			get {
				return this.exif;
			}
			set {
				this.exif = value;
			}
		}

		public bool SaveImageToDisk(Image image, ImageFormat format, IFilenameProvider fileNameProvider) {
			string fileName = fileNameProvider.GetFileName();

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			// find the encoder with the image/jpeg mime-type
			ImageCodecInfo imageCodec = null;
			foreach (ImageCodecInfo codec in codecs) {
				if (codec.MimeType == "image/jpeg") {
					imageCodec = codec;
				}
			}

			// Create a collection of encoder parameters (we only need one in the collection)
			EncoderParameters encoder = new EncoderParameters();

			// We'll save images with 25%, 50%, 75% and 100% quality as compared with the original
			encoder.Param[0] = new EncoderParameter(Encoder.Quality, compressionRatio);

			try {
				image.Save(fileName, imageCodec, encoder);

				addExif(fileName, (uint)image.Width, (uint)image.Height);

				Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " saved " + fileName + " at " + DateTime.Now + ".");

				return true;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return false;
			}
		}

		private void addExif(string fileName, uint width, uint height) {
			if (this.exif == null) {
				return;
			}

			ExifMetadata targetExif = new ExifMetadata(new Uri(fileName, UriKind.Absolute));
			targetExif.CameraModel = this.exif.CameraModel;
			targetExif.ColorRepresentation = this.exif.ColorRepresentation;
			targetExif.CreationSoftware = "Sea Turtle Batch Image Processor";
			targetExif.DateImageTaken = this.exif.DateImageTaken;
			targetExif.EquipmentManufacturer = this.exif.EquipmentManufacturer;
			targetExif.ExifVersion = "2.2";
			targetExif.ExposureCompensation = this.exif.ExposureCompensation;
			targetExif.ExposureMode = this.exif.ExposureMode;
			targetExif.ExposureTime = this.exif.ExposureTime;
			targetExif.FlashMode = this.exif.FlashMode;
			targetExif.FocalLength = this.exif.FocalLength;
			targetExif.Height = height;
			targetExif.HorizontalResolution = this.exif.HorizontalResolution;
			targetExif.IsoSpeed = this.exif.IsoSpeed;
			targetExif.LensAperture = this.exif.LensAperture;
			targetExif.LightSource = this.exif.LightSource;
			targetExif.MeteringMode = this.exif.MeteringMode;
			targetExif.VerticalResolution = this.exif.VerticalResolution;
			targetExif.Width = width;
			targetExif.Latitude = this.exif.Latitude;
			targetExif.Longitude = this.exif.Longitude;
			targetExif.Altitude = this.exif.Altitude;

			targetExif.SaveExif();
		}
	}
}
