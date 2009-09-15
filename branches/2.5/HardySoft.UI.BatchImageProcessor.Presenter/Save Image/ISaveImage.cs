using System;
using System.Drawing;
using System.Drawing.Imaging;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface ISaveImage {
		bool SaveImageToDisk(Image image, string fileName, ImageFormat format);

		ExifMetadata Exif {
			get;
			set;
		}
	}
}