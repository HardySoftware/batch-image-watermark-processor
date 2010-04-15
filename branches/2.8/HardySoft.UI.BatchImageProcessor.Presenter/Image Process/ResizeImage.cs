using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public abstract class ResizeImage : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public abstract Image ProcessImage(Image input, ProjectSetting ps);

		protected Image ResizeImageJob(Image input, System.Windows.Size newSize, InterpolationMode mode) {
			try {
				int originalWidth, originalHeight;
				originalWidth = input.Width;
				originalHeight = input.Height;

				Bitmap newImage = new Bitmap((int)newSize.Width, (int)newSize.Height);
				Graphics g = Graphics.FromImage(newImage);

				g.InterpolationMode = mode;
				g.DrawImage(input, new Rectangle(0, 0, newImage.Width, newImage.Height),
					0, 0, input.Width, input.Height, GraphicsUnit.Pixel);
				g.Dispose();
				return (Image)newImage;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}
	}
}
