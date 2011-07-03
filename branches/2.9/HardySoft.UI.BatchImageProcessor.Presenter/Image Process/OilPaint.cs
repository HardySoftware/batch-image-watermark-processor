using System;
using System.Diagnostics;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class OilPaint : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				int width = input.Width;
				int height = input.Height;

				Bitmap bmp = new Bitmap(input);

				Color color;

				Random rnd = new Random();

				for (int i = 0; i < width - 5; i++) {
					for (int j = 0; j < height - 5; j++) {
						int a = rnd.Next(5);
						color = bmp.GetPixel(i + a, j + a);
						bmp.SetPixel(i, j, color);
					}
				}

				Graphics graphic = Graphics.FromImage(bmp);
				graphic.DrawImage(bmp, new Rectangle(0, 0, width, height));
				graphic.Dispose();

				return (Image)bmp;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}
	}
}
