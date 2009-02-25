using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class NegativeImage : IProcess {
		public Image ProcessImage(Image input, ProjectSetting ps) {
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
		}
	}
}
