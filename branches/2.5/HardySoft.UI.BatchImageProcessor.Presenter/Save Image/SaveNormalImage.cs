using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using HardySoft.CC;
using HardySoft.CC.ExceptionLog;
using HardySoft.CC.Transformer;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	class SaveNormalImage : ISaveImage {
		public bool SaveImageToDisk(Image image, string fileName, ImageFormat format) {
			try {
				image.Save(fileName, format);

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

		public bool EnableDebug {
			get;
			set;
		}
	}
}
