using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using HardySoft.CC;
using HardySoft.UI.BatchImageProcessor.Model;
using System.IO;
using HardySoft.UI.BatchImageProcessor.Model.Exif;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class ApplyWatermarkText : Watermark {
		private List<ExifContainerItem> exifContainer;
		private string dateTimeStringFormat;
		private int watermarkIndex;

		public ApplyWatermarkText(List<ExifContainerItem> exifContainer, string dateTimeStringFormat,
			int watermarkIndex) {
			this.exifContainer = exifContainer;
			this.dateTimeStringFormat = dateTimeStringFormat;
			this.watermarkIndex = watermarkIndex;
		}

		// TODO examine to add text to a transparent GIF image still works.
		public override Image ProcessImage(Image input, ProjectSetting ps) {
			try {
				if (ps.WatermarkCollection.Count < watermarkIndex) {
					return input;
				}

				WatermarkText wt = ps.WatermarkCollection[this.watermarkIndex] as WatermarkText;

				if (wt == null) {
					return input;
				}

#if DEBUG
				Debug.WriteLine("Current Thread: "
					+ Thread.CurrentThread.ManagedThreadId + ","
					+ " Image File Name: " + this.ImageFileName + ","
					+ " Watermark Text index: " + watermarkIndex);
#endif

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

				string textToDraw = wt.Text;
				List<ExifContainerItem> tagsFound = this.FindExifTags(textToDraw);

				if (tagsFound != null && tagsFound.Count > 0 && !string.IsNullOrEmpty(this.ImageFileName)) {
					// at least one Exif tag is found
					textToDraw = ConvertExifTagsToText(textToDraw, tagsFound);
				}

				textToDraw = this.ConvertControlTagsToText(textToDraw);

				// create a bitmap we can use to work out the size of the text,
				// we will then create a new bitmap that is the right size.
				// we also use this to record the default resolution
				Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
				fHRes = bmp.HorizontalResolution;
				fVRes = bmp.VerticalResolution;
				Graphics g = Graphics.FromImage(bmp);
				StringFormat format = new StringFormat();
				format.Alignment = wt.WatermarkTextAlignment;
				SizeF sf = g.MeasureString(textToDraw, wt.WatermarkTextFont,
					Int32.MaxValue, format);
				g.Dispose();
				bmp.Dispose();

				// get the width and height of the text
				textWidth = Math.Max(2, (int)Math.Ceiling(sf.Width));
				textHeight = Math.Max(2, (int)Math.Ceiling(sf.Height));

				RotationSize rs = CalculateRotationSize(textWidth, textHeight, wt.WatermarkRotateAngle);
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
				switch (wt.WatermarkPosition) {
					case ContentAlignment.TopLeft:
					case ContentAlignment.TopCenter:
					case ContentAlignment.TopRight:
						//yAfterOffset = definition.TextPadding.Top;
						yAfterOffset = wt.Padding;
						break;
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.MiddleRight:
						//yAfterOffset = (bmp.Height - rotatedHeight) / 2 + definition.TextPadding.Top - definition.TextPadding.Bottom;
						yAfterOffset = (bmp.Height - rotatedTextHeight) / 2 + wt.Padding - wt.Padding;
						break;
					case ContentAlignment.BottomLeft:
					case ContentAlignment.BottomCenter:
					case ContentAlignment.BottomRight:
						//yAfterOffset = (bmp.Height - rotatedHeight) - definition.TextPadding.Bottom;
						yAfterOffset = (bmp.Height - rotatedTextHeight) - wt.Padding;
						break;
				}
				switch (wt.WatermarkPosition) {
					case ContentAlignment.TopLeft:
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.BottomLeft:
						//xAfterOffset = definition.TextPadding.Left;
						xAfterOffset = wt.Padding;
						break;
					case ContentAlignment.TopCenter:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.BottomCenter:
						//xAfterOffset = (bmp.Width - rotatedWidth) / 2 + definition.TextPadding.Left - definition.TextPadding.Right;
						xAfterOffset = (bmp.Width - rotatedTextWidth) / 2 + wt.Padding - wt.Padding;
						break;
					case ContentAlignment.TopRight:
					case ContentAlignment.MiddleRight:
					case ContentAlignment.BottomRight:
						//xAfterOffset = (bmp.Width - rotatedWidth) - definition.TextPadding.Right;
						xAfterOffset = (bmp.Width - rotatedTextWidth) - wt.Padding;
						break;
				}

				// create a new transformation matrix to do the rotation and corresponding translation
				Matrix matrix = new Matrix();
				// translation to position the text as required by HAlignment and VAlignment
				matrix.Translate(xAfterOffset, yAfterOffset);
				// translation to bring the rotation back to view
				matrix.Translate(iHOffset, iVOffset);
				// transformation to rotate the text
				matrix.Rotate(wt.WatermarkRotateAngle);
				if (wt.WatermarkTextAlignment == StringAlignment.Center) {
					// transformation to cope with non left aligned text
					matrix.Translate(textWidth / 2, 0);
				} else if (wt.WatermarkTextAlignment == StringAlignment.Far) {
					matrix.Translate(textWidth, 0);
				}

				// apply the transformation to the graphics object and write out the text
				g.Transform = matrix;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.TextRenderingHint = TextRenderingHint.AntiAlias;
				// draw a shadow with semi transparent black
				g.DrawString(textToDraw,
					wt.WatermarkTextFont,
					new SolidBrush(Color.FromArgb(153, 0, 0, 0)), 1, 1, format);
				// draw the sctual text
				g.DrawString(textToDraw,
					wt.WatermarkTextFont,
					new SolidBrush(wt.WatermarkTextColor), 0, 0, format);
				g.Transform = matrix;
				g.Dispose();

				return bmp;
			} catch (Exception ex) {
				Trace.TraceError(ex.ToString());
				return input;
			}
		}

		private string ConvertExifTagsToText(string input, List<ExifContainerItem> tagsFound) {
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
					} else if (tagFound.Property.PropertyType == typeof(DateTime?)) {
#if DEBUG
						Debug.WriteLine("Current Thread "
							+ System.Threading.Thread.CurrentThread.ManagedThreadId + " Culture "
							+ System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
							+ " in ApplyWatermarkText.");
#endif
						// date time value, use format string defined in preference window to overwrite
						DateTime? d = (DateTime?)propertyValue;
						if (d.HasValue) {
							localizedValue = d.Value.ToString(this.dateTimeStringFormat);
						} else {
							localizedValue = string.Empty;
						}
					} else {
						localizedValue = propertyValue.ToString();
					}

					if (!string.IsNullOrEmpty(tagFound.ValueFormat)) {
						localizedValue = string.Format(tagFound.ValueFormat, localizedValue);
					}
				}
				input = input.Replace(tag, localizedValue);
			}
			return input;
		}

		/// <summary>
		/// Get all valid EXIF tags in watermark text.
		/// </summary>
		/// <param name="textToDraw"></param>
		/// <returns></returns>
		private List<ExifContainerItem> FindExifTags(string textToDraw) {
			string[] tagsFound = Parser.TagParser(textToDraw, "[[", "]]");
			if (tagsFound == null || tagsFound.Length == 0) {
				return null;
			} else {
				List<ExifContainerItem> tags = new List<ExifContainerItem>();
				foreach (ExifContainerItem exifItem in this.exifContainer) {
					for (int i = 0; i < tagsFound.Length; i++) {
						if (string.Compare(exifItem.ExifTag, tagsFound[i], false) == 0) {
							// found at least one tag match
							tags.Add(exifItem);
						}
					}
				}

				return tags;
			}
		}

		/// <summary>
		/// Finds all controlling tag "{{...}}" and translate them into real texts.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private string ConvertControlTagsToText(string input) {
			string[] tagsFound = Parser.TagParser(input, "{{", "}}");
			if (tagsFound == null || tagsFound.Length == 0) {
				// no controlling tags, remove the tag.
				return input;
			} else {
				// Controlling file is same name as image file, but with .txt as extension
				FileInfo fi = new FileInfo(this.ImageFileName);
				string controllingFile = Path.Combine(fi.DirectoryName, fi.Name.Replace(fi.Extension, string.Empty) + ".txt");
				if (File.Exists(controllingFile)) {
					string[] controllingLines = File.ReadAllLines(controllingFile);

					foreach (string tag in tagsFound) {
						string completeTag = "{{" + tag + "}}";
						string controllingLine = (from c in controllingLines where c.StartsWith(completeTag) select c.Replace(completeTag, string.Empty).Trim()).FirstOrDefault();

						input = input.Replace(completeTag, controllingLine);
					}
				} else {
					// controlling file is not available

					foreach (string tag in tagsFound) {
						string completeTag = "{{" + tag + "}}";

						input = input.Replace(completeTag, string.Empty);
					}
				}

				return input;
			}
		}
	}
}