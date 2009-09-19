using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class GrayScale : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				Bitmap bmp = new Bitmap(input.Width, input.Height);
				Graphics graphic = Graphics.FromImage(bmp);

				//Gilles Khouzams colour corrected grayscale shear
				ColorMatrix matrix = new ColorMatrix(new float[][]{
				new float[]{0.3f,0.3f,0.3f,0,0},
				new float[]{0.59f,0.59f,0.59f,0,0},
				new float[]{0.11f,0.11f,0.11f,0,0},
				new float[]{0,0,0,1,0,0},
				new float[]{0,0,0,0,1,0},
				new float[]{0,0,0,0,0,1}}
				);

				ImageAttributes attributes = new ImageAttributes();
				attributes.SetColorMatrix(matrix);
				graphic.DrawImage(input, new Rectangle(0, 0, input.Width, input.Height),
					0, 0, input.Width, input.Height, GraphicsUnit.Pixel, attributes);
				graphic.Dispose();
				return (Image)bmp;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		} 
	}
}
