using System;
using System.Diagnostics;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class Relief : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				int width = input.Width;
				int height = input.Height;

				Bitmap bmp = new Bitmap(input);

				Color pixel1, pixel2;

				for (int x = 0; x < width - 1; x++) {
					for (int y = 0; y < height - 1; y++) {
						int r = 0, g = 0, b = 0;
						pixel1 = bmp.GetPixel(x, y);
						pixel2 = bmp.GetPixel(x + 1, y + 1);

						r = Math.Abs(pixel1.R - pixel2.R + 128);
						g = Math.Abs(pixel1.G - pixel2.G + 128);
						b = Math.Abs(pixel1.B - pixel2.B + 128);

						if (r > 255) {
							r = 255;
						}

						if (g > 255) {
							g = 255;
						}

						if (b > 255) {
							b = 255;
						}

						bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
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
