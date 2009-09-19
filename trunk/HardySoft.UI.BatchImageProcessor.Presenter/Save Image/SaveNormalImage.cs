using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class SaveNormalImage : ISaveImage {
		public bool SaveImageToDisk(Image image, ImageFormat format, IFilenameProvider fileNameProvider) {
			try {
				string fileName = fileNameProvider.GetFileName();

				image.Save(fileName, format);

				return true;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return false;
			}
		}

		public ExifMetadata Exif {
			get {
				throw new NotImplementedException("Not Implemented");
			}
			set {
				throw new NotImplementedException("Not Implemented");
			}
		}
	}
}
