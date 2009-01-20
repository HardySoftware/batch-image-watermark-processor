using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Diagnostics;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public abstract class ResizeImage : IProcess {
		public abstract Image ProcessImage(Image input, ProjectSetting ps);

		protected Image ResizeImageJob(Image input, uint newSize, InterpolationMode mode) {
			int originalWidth, originalHeight;
			originalWidth = input.Width;
			originalHeight = input.Height;

			float ratio = (float)originalWidth / (float)originalHeight;
			uint newWidth, newHeight;
			if (originalWidth >= originalHeight) {
				// wider picture
				newWidth = newSize;
				newHeight = (uint)((float)newSize / ratio);
			} else {
				// taller picture
				newWidth = (uint)((float)newSize * ratio);
				newHeight = newSize;
			}
			Bitmap newImage = new Bitmap((int)newWidth, (int)newHeight);
			Graphics g = Graphics.FromImage(newImage);

			g.InterpolationMode = mode;
			g.DrawImage(input, new Rectangle(0, 0, newImage.Width, newImage.Height),
				0, 0, input.Width, input.Height, GraphicsUnit.Pixel);
			g.Dispose();
			return (Image)newImage;
		}
	}
}
