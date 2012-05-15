using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class SaveNormalImage : ISaveImage {
		public bool SaveImageToDisk(Image image, ImageFormat format, IFilenameProvider fileNameProvider) {
			try {
				string fileName = fileNameProvider.GetFileName();

				image.Save(fileName, format);

				Trace.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " saved " + fileName + " at " + DateTime.Now + ".");

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
