using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface ISaveImage {
		bool SaveImageToDisk(Image image, string fileName, ImageFormat format);
	}
}