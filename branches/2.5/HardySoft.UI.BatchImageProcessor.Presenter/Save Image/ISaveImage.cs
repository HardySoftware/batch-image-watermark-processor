using System;
using System.Drawing;
using System.Drawing.Imaging;

using HardySoft.UI.BatchImageProcessor.Model;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface ISaveImage {
		bool SaveImageToDisk(Image image, ImageFormat format, IFilenameProvider fileNameProvider);

		ExifMetadata Exif {
			get;
			set;
		}
	}
}