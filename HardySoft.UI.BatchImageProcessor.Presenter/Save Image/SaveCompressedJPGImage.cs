using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using HardySoft.CC;
using HardySoft.CC.ExceptionLog;
using HardySoft.CC.Transformer;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to save JPG image with certain compression ratio.
	/// </summary>
	class SaveCompressedJPGImage : ISaveImage {
		private long compressionRatio;

		public SaveCompressedJPGImage(long compressionRatio) {
			this.compressionRatio = compressionRatio;
		}

		public bool EnableDebug {
			get;
			set;
		}

		public bool SaveImageToDisk(Image image, string fileName, ImageFormat format) {
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
				return true;
			} catch (Exception ex) {
				if (this.EnableDebug) {
					string logFile = Formatter.FormalizeFolderName(Directory.GetCurrentDirectory()) + @"logs\SeaTurtle_Error.log";
					string logXml = Serializer.Serialize<ExceptionContainer>(ExceptionLogger.GetException(ex));

					HardySoft.CC.File.FileAccess.AppendFile(logFile, logXml);
				}
				return false;
			}
		}
	}
}
