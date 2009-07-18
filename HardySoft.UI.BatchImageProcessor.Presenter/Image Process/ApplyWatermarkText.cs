using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

using HardySoft.CC;
using HardySoft.CC.ExceptionLog;
using HardySoft.CC.Transformer;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ApplyWatermarkText : Watermark {
		private List<ExifContainerItem> exifContainer;

		public ApplyWatermarkText(List<ExifContainerItem> exifContainer) {
			this.exifContainer = exifContainer;
		}

		// TODO examine to add text to a transparent GIF image still works.
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				int iHOffset = 0;
				int iVOffset = 0;
				int rotatedTextHeight = 0;
				int rotatedTextWidth = 0;
				int iBmpHeight = 0;
				int iBmpWidth = 0;
				float fHRes = 0;
				float fVRes = 0;
				int textWidth = 0;
				int textHeight = 0;
				int xAfterOffset = 0;
				int yAfterOffset = 0;

				string textToDraw = ps.Watermark.WatermarkText;
				List<ExifContainerItem> tagsFound = foundExifTags(textToDraw);

				if (tagsFound != null && tagsFound.Count > 0 && !string.IsNullOrEmpty(this.ImageFileName)) {
					// at least one Exif tag is found
					ExifMetadata meta = new ExifMetadata(new Uri(this.ImageFileName));

					foreach (ExifContainerItem tagFound in tagsFound) {
						// replace actual value from Exif in watermark text
						string tag = "[[" + tagFound.ExifTag + "]]";
						object propertyValue = tagFound.Property.GetValue(meta, null);
						string localizedValue;
						if (propertyValue == null) {
							localizedValue = string.Empty;
						} else {
							if (tagFound.Property.PropertyType.IsEnum && tagFound.EnumValueTranslation != null) {
								localizedValue = propertyValue.ToString();

								foreach (KeyValuePair<string, string> enumItem in tagFound.EnumValueTranslation) {
									if (localizedValue == enumItem.Key) {
										localizedValue = enumItem.Value;
										break;
									}
								}
							} else {
								localizedValue = propertyValue.ToString();
							}

							if (!string.IsNullOrEmpty(tagFound.ValueFormat)) {
								localizedValue = string.Format(tagFound.ValueFormat, localizedValue);
							}
						}
						textToDraw = textToDraw.Replace(tag, localizedValue);
					}
				}

				// create a bitmap we can use to work out the size of the text,
				// we will then create a new bitmap that is the right size.
				// we also use this to record the default resolution
				Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
				fHRes = bmp.HorizontalResolution;
				fVRes = bmp.VerticalResolution;
				Graphics g = Graphics.FromImage(bmp);
				StringFormat format = new StringFormat();
				format.Alignment = ps.Watermark.WatermarkTextAlignment;
				SizeF sf = g.MeasureString(textToDraw, ps.Watermark.WatermarkTextFont,
					Int32.MaxValue, format);
				g.Dispose();
				bmp.Dispose();

				// get the width and height of the text
				textWidth = Math.Max(2, (int)Math.Ceiling(sf.Width));
				textHeight = Math.Max(2, (int)Math.Ceiling(sf.Height));

				RotationSize rs = CalculateRotationSize(textWidth, textHeight, ps.Watermark.WatermarkTextRotateAngle);
				iHOffset = rs.HorizontalOffset;
				iVOffset = rs.VerticalOffset;
				rotatedTextWidth = rs.RotatedWidth;
				rotatedTextHeight = rs.RotatedHeight;

				bmp = new Bitmap(input);
				iBmpWidth = bmp.Width;
				iBmpHeight = bmp.Height;
				g = Graphics.FromImage(bmp);

				// determine the offset needed to position the text in the right place 
				// in the background if it is not the same size as we calculate was needed for the text
				// adding in the padding where necessary and
				// remembering to adjust for differences in padding between left/right top/bottom etc.
				switch (ps.Watermark.WatermarkTextPosition) {
					case ContentAlignment.TopLeft:
					case ContentAlignment.TopCenter:
					case ContentAlignment.TopRight:
						//yAfterOffset = definition.TextPadding.Top;
						yAfterOffset = Padding;
						break;
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.MiddleRight:
						//yAfterOffset = (bmp.Height - rotatedHeight) / 2 + definition.TextPadding.Top - definition.TextPadding.Bottom;
						yAfterOffset = (bmp.Height - rotatedTextHeight) / 2 + Padding - Padding;
						break;
					case ContentAlignment.BottomLeft:
					case ContentAlignment.BottomCenter:
					case ContentAlignment.BottomRight:
						//yAfterOffset = (bmp.Height - rotatedHeight) - definition.TextPadding.Bottom;
						yAfterOffset = (bmp.Height - rotatedTextHeight) - Padding;
						break;
				}
				switch (ps.Watermark.WatermarkTextPosition) {
					case ContentAlignment.TopLeft:
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.BottomLeft:
						//xAfterOffset = definition.TextPadding.Left;
						xAfterOffset = Padding;
						break;
					case ContentAlignment.TopCenter:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.BottomCenter:
						//xAfterOffset = (bmp.Width - rotatedWidth) / 2 + definition.TextPadding.Left - definition.TextPadding.Right;
						xAfterOffset = (bmp.Width - rotatedTextWidth) / 2 + Padding - Padding;
						break;
					case ContentAlignment.TopRight:
					case ContentAlignment.MiddleRight:
					case ContentAlignment.BottomRight:
						//xAfterOffset = (bmp.Width - rotatedWidth) - definition.TextPadding.Right;
						xAfterOffset = (bmp.Width - rotatedTextWidth) - Padding;
						break;
				}

				// create a new transformation matrix to do the rotation and corresponding translation
				Matrix matrix = new Matrix();
				// translation to position the text as required by HAlignment and VAlignment
				matrix.Translate(xAfterOffset, yAfterOffset);
				// translation to bring the rotation back to view
				matrix.Translate(iHOffset, iVOffset);
				// transformation to rotate the text
				matrix.Rotate(ps.Watermark.WatermarkTextRotateAngle);
				if (ps.Watermark.WatermarkTextAlignment == StringAlignment.Center) {
					// transformation to cope with non left aligned text
					matrix.Translate(textWidth / 2, 0);
				} else if (ps.Watermark.WatermarkTextAlignment == StringAlignment.Far) {
					matrix.Translate(textWidth, 0);
				}

				// apply the transformation to the graphics object and write out the text
				g.Transform = matrix;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.TextRenderingHint = TextRenderingHint.AntiAlias;
				// draw a shadow with semi transparent black
				g.DrawString(textToDraw,
					ps.Watermark.WatermarkTextFont,
					new SolidBrush(Color.FromArgb(153, 0, 0, 0)), 1, 1, format);
				// draw the sctual text
				g.DrawString(textToDraw,
					ps.Watermark.WatermarkTextFont,
					new SolidBrush(ps.Watermark.WatermarkTextColor), 0, 0, format);
				g.Transform = matrix;
				g.Dispose();

				return bmp;
			} catch (Exception ex) {
				if (this.EnableDebug) {
					string logFile = Formatter.FormalizeFolderName(Directory.GetCurrentDirectory()) + @"logs\SeaTurtle_Error.log";
					string logXml = Serializer.Serialize<ExceptionContainer>(ExceptionLogger.GetException(ex));

					HardySoft.CC.File.FileAccess.AppendFile(logFile, logXml);
				}
				return input;
			}
		}

		/// <summary>
		/// Get all valid image tags in watermark text.
		/// </summary>
		/// <param name="textToDraw"></param>
		/// <returns></returns>
		private List<ExifContainerItem> foundExifTags(string textToDraw) {
			string[] tagsFound = Parser.TagParser(textToDraw, "[[", "]]");
			if (tagsFound == null || tagsFound.Length == 0) {
				return null;
			} else {
				List<ExifContainerItem> tags = new List<ExifContainerItem>();
				foreach (ExifContainerItem exifItem in this.exifContainer) {
					for (int i = 0; i < tagsFound.Length; i++) {
						if (string.Compare(exifItem.ExifTag, tagsFound[i], false) == 0) {
							// found at least one tage match
							tags.Add(exifItem);
						}
					}
				}

				return tags;
			}
		}
	}
}