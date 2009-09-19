using System;
using System.Diagnostics;
using System.Drawing;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class AddBorder : IProcess {
		public string ImageFileName {
			get;
			set;
		}

		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				int innerBorderWidth = 1;
				int borderWidth = (int)ps.BorderSetting.BorderWidth;
				Bitmap bmp = new Bitmap(input.Width + (borderWidth + innerBorderWidth) * 2,
					input.Height + (borderWidth + innerBorderWidth) * 2);
				Graphics g = Graphics.FromImage(bmp);
				Color c = Color.FromArgb(ps.BorderSetting.BorderColor.A,
					ps.BorderSetting.BorderColor.R,
					ps.BorderSetting.BorderColor.G,
					ps.BorderSetting.BorderColor.B);
				SolidBrush brush = new SolidBrush(c);
				g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);

				brush = new SolidBrush(Color.White);
				g.FillRectangle(brush, borderWidth, borderWidth,
					input.Width + innerBorderWidth * 2,
					input.Height + innerBorderWidth * 2);

				g.DrawImage(input, borderWidth + innerBorderWidth, borderWidth + innerBorderWidth);
				g.Dispose();
				return (Image)bmp;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}
	}
}