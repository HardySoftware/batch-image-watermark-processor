using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

using HardySoft.CC;
using HardySoft.CC.ExceptionLog;
using HardySoft.CC.Transformer;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ApplyWatermarkText : IProcess {
		public bool EnableDebug {
			get;
			set;
		}

		// TODO examine to add text to a transparent GIF image still works.
		public Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				int imageWidth = input.Width;
				int imageHeight = input.Height;

				Bitmap bmp = new Bitmap(imageWidth, imageHeight);
				Graphics graphic = Graphics.FromImage(bmp);
				//Set the rendering quality for this Graphics object
				graphic.SmoothingMode = SmoothingMode.AntiAlias;

				//Draws the photo Image object at original size to the graphics object.
				graphic.DrawImage(
					input,                               // Photo Image object
					new Rectangle(0, 0, imageWidth, imageHeight), // Rectangle structure
					0,                                      // x-coordinate of the portion of the source image to draw. 
					0,                                      // y-coordinate of the portion of the source image to draw. 
					imageWidth,                             // Width of the portion of the source image to draw. 
					imageHeight,                            // Height of the portion of the source image to draw. 
					GraphicsUnit.Pixel);                    // Units of measure 

				Font font = ps.Watermark.WatermarkTextFont;
				SizeF fontSize = new SizeF();
				fontSize = graphic.MeasureString(ps.Watermark.WatermarkText, font);

				int yPixlesFromBottom = 0;
				float yPosFromBottom = 0;
				float xCenterOfImg = 0;

				switch (ps.Watermark.WatermarkTextPosition) {
					case ContentAlignment.BottomCenter:
						//Since all photographs will have varying heights, determine a 
						//position 5% from the bottom of the image
						yPixlesFromBottom = (int)(imageHeight * .05);

						//Now that we have a point size use the m_Copyrights string height 
						//to determine a y-coordinate to draw the string of the photograph
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));

						//Determine its x-coordinate by calculating the center of the width of the image
						xCenterOfImg = (imageWidth / 2);
						break;

					case ContentAlignment.BottomLeft:
						yPixlesFromBottom = (int)(imageHeight * .05);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (fontSize.Width / 2) + 10;
						break;

					case ContentAlignment.BottomRight:
						yPixlesFromBottom = (int)(imageHeight * .05);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
						break;

					case ContentAlignment.MiddleCenter:
						yPixlesFromBottom = (int)(imageHeight * .50);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (imageWidth / 2);
						break;

					case ContentAlignment.MiddleLeft:
						yPixlesFromBottom = (int)(imageHeight * .50);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (fontSize.Width / 2) + 10;
						break;

					case ContentAlignment.MiddleRight:
						yPixlesFromBottom = (int)(imageHeight * .50);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
						break;

					case ContentAlignment.TopCenter:
						yPixlesFromBottom = (int)(imageHeight * .95);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (imageWidth / 2);
						break;

					case ContentAlignment.TopLeft:
						yPixlesFromBottom = (int)(imageHeight * .95);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (fontSize.Width / 2) + 10;
						break;

					case ContentAlignment.TopRight:
						yPixlesFromBottom = (int)(imageHeight * .95);
						yPosFromBottom = ((imageHeight - yPixlesFromBottom) - (fontSize.Height / 2));
						xCenterOfImg = (imageWidth - (fontSize.Width / 2)) - 10;
						break;
				}

				//Define the text layout by setting the text watermarkTextAlignment to centered
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;

				//define a Brush which is semi transparent black (Alpha set to 153)
				SolidBrush semiTransparentBrushBlack = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

				//Draw the copyright string
				graphic.DrawString(ps.Watermark.WatermarkText,	//string of text
					font,										//font
					semiTransparentBrushBlack,							//Brush
					new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
					stringFormat);

				//define a Brush which is semi transparent white (Alpha set to 153)
				SolidBrush semiTransparentBrushWhite = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

				//Draw the copyright string a second time to create a shadow effect
				//Make sure to move this text 1 pixel to the right and 1 pixel down
				graphic.DrawString(ps.Watermark.WatermarkText,                 //string of text
					font,                                   //font
					semiTransparentBrushWhite,                           //Brush
					new PointF(xCenterOfImg, yPosFromBottom),  //Position
					stringFormat);                               //Text watermarkTextAlignment

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