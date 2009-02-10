using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class SaveNormalImage : ISaveImage {
		public bool SaveImageToDisk(Image image, string fileName, ImageFormat format) {
			try {
				image.Save(fileName, format);

				return true;
			} catch {
				// TODO add error handling logic here
				return false;
			}
		}
	}
}
