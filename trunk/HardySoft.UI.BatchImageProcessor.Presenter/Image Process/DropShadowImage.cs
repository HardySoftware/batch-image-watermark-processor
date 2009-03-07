using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class DropShadowImage : IProcess {
		public Image ProcessImage(Image input, ProjectSetting ps) {
			int originalWidth = input.Width;
			int originalHeight = input.Height;
			int depth = ps.DropShadowSetting.ShadowDepth;
			// size with shadow included
			Bitmap bmp = new Bitmap(originalWidth + depth, originalHeight + depth);
			//bmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);
			Graphics graphic = Graphics.FromImage(bmp);

			Rectangle rc = new Rectangle(0, 0, originalWidth + depth, originalHeight + depth);

			//calculate the opacities 
			Color darkShadow = ps.DropShadowSetting.DropShadowColor;
			Color lightShadow = Color.FromArgb(0,
				ps.DropShadowSetting.DropShadowColor.R,
				ps.DropShadowSetting.DropShadowColor.G,
				ps.DropShadowSetting.DropShadowColor.B);

			//Create a brush that will create a softshadow circle 
			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(0, 0, 2 * depth, 2 * depth);
			PathGradientBrush pgb = new PathGradientBrush(gp);
			pgb.CenterColor = darkShadow;
			pgb.SurroundColors = new Color[] { lightShadow };

			//generate a softshadow pattern that can be used to paint the shadow 
			Bitmap patternBmp = new Bitmap(2 * depth, 2 * depth);
			Graphics g = Graphics.FromImage(patternBmp);
			g.FillEllipse(pgb, 0, 0, 2 * depth, 2 * depth);
			g.Dispose();
			pgb.Dispose();

			SolidBrush sb = new SolidBrush(ps.DropShadowSetting.BackgroundColor);
			graphic.FillRectangle(sb, rc);
			sb.Dispose();

			switch (ps.DropShadowSetting.ShadowLocation) {
				case ContentAlignment.BottomLeft:
					//bottom side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
						depth, depth, 1, depth, GraphicsUnit.Pixel);

					//bottom left corner 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Bottom - depth, depth, depth),
						0, depth, depth, depth, GraphicsUnit.Pixel);

					//left side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
						0, depth, depth, 1, GraphicsUnit.Pixel);

					// draw input photo
					graphic.DrawImage(input, depth, 0, originalWidth, originalHeight);

					break;
				case ContentAlignment.BottomRight:
					//right side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Top + depth,
						depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
						GraphicsUnit.Pixel);

					//bottom right corner 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Bottom - depth, depth, depth),
						depth, depth, depth, depth, GraphicsUnit.Pixel);

					//bottom side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
						depth, depth, 1, depth, GraphicsUnit.Pixel);

					// draw input photo
					graphic.DrawImage(input, 0, 0, originalWidth, originalHeight);

					break;
				case ContentAlignment.TopLeft:
					//top left corner 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Top, depth, depth), 0, 0, depth, depth,
						GraphicsUnit.Pixel);

					//top side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
						depth, 0, 1, depth, GraphicsUnit.Pixel);

					//left side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
						0, depth, depth, 1, GraphicsUnit.Pixel);

					// draw input photo
					graphic.DrawImage(input, depth, depth, originalWidth, originalHeight);

					break;
				case ContentAlignment.TopRight:
					//top side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
						depth, 0, 1, depth, GraphicsUnit.Pixel);

					//top right corner 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Top, depth, depth),
						depth, 0, depth, depth, GraphicsUnit.Pixel);

					//right side 
					graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Top + depth,
						depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
						GraphicsUnit.Pixel);

					// draw input photo
					graphic.DrawImage(input, 0, depth, originalWidth, originalHeight);

					break;
			}

			#region All 8 spots, commented
			/*
            //top left corner 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Top, depth, depth), 0, 0, depth, depth,
                GraphicsUnit.Pixel);

            //top side 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2 * depth), depth),
                depth, 0, 1, depth, GraphicsUnit.Pixel);

            //top right corner 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Top, depth, depth),
                depth, 0, depth, depth, GraphicsUnit.Pixel);

            //right side 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Top + depth,
                depth, rc.Height - (2 * depth)), depth, depth, depth, 1,
                GraphicsUnit.Pixel);

            //bottom right corner 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Right - depth, rc.Bottom - depth, depth, depth),
                depth, depth, depth, depth, GraphicsUnit.Pixel);

            //bottom side 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2 * depth), depth),
                depth, depth, 1, depth, GraphicsUnit.Pixel);

            //bottom left corner 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Bottom - depth, depth, depth),
                0, depth, depth, depth, GraphicsUnit.Pixel);

            //left side 
            graphic.DrawImage(patternBmp, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2 * depth)),
                0, depth, depth, 1, GraphicsUnit.Pixel);*/
			#endregion

			patternBmp.Dispose();
			graphic.Dispose();
			return (Image)bmp;
		}
	}
}
