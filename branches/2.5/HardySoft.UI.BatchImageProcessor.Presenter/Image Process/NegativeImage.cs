using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using HardySoft.CC;
using HardySoft.CC.ExceptionLog;
using HardySoft.CC.Transformer;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class NegativeImage : IProcess {
		public bool EnableDebug {
			get;
			set;
		}

		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				Bitmap bmp = new Bitmap(input.Width, input.Height);
				ImageAttributes attributes = new ImageAttributes();
				ColorMatrix matrix = new ColorMatrix();

				matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = -1;

				//matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = 0.99f;
				//matrix.Matrix33 = matrix.Matrix44 = 1;
				//matrix.Matrix40 = matrix.Matrix41 = matrix.Matrix42 = .04f;

				attributes.SetColorMatrix(matrix);

				Graphics shadowGraphics = Graphics.FromImage(bmp);
				shadowGraphics.DrawImage(input, new Rectangle(0, 0, input.Width, input.Height),
					0, 0, input.Width, input.Height, GraphicsUnit.Pixel, attributes);
				shadowGraphics.Dispose();
				return (Image)bmp;
			} catch (Exception ex) {
				if (this.EnableDebug) {
					string logFile = Formatter.FormalizeFolderName(Directory.GetCurrentDirectory()) + @"logs\SeaTurtle_Error.log";
					string logXml = Serializer.Serialize<ExceptionContainer>(ExceptionLogger.GetException(ex));

					HardySoft.CC.File.FileAccess.AppendFile(logFile, logXml);
				}
				return input;
			}
		}
	}
}