using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to save JPG image with certain compression ratio.
	/// </summary>
	class SaveCompressedJPGImage : ISaveImage {
		private long compressionRatio;

		public SaveCompressedJPGImage(long compressionRatio) {
			this.compressionRatio = compressionRatio;
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
			} catch {
				// TODO add error handling logic here
				return false;
			}
		}
	}
}
