using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using HardySoft.UI.BatchImageProcessor.Model.ModelValidators;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class WatermarkText : WatermarkBase {
		public WatermarkText() : base() {
			this.watermarkPosition = ContentAlignment.BottomRight;
			this.watermarkTextColor = System.Drawing.Color.FromArgb(153, 255, 255, 255);
			this.watermarkTextAlignment = StringAlignment.Center;
			this.watermarkTextFont = SystemFonts.DialogFont;
		}

		private string text;
		[MacroTagValidator(typeof(ExifMetadata), MessageTemplate = "Validation_ExifMacroError||{0}", Tag = "Warning")]
		public string Text {
			get {
				return this.text;
			}
			set {
				if (string.Compare(this.text, value, false) != 0) {
					this.text = value;
					notify("Text");
				}
			}
		}

		private Font watermarkTextFont;
		public Font WatermarkTextFont {
			get {
				return this.watermarkTextFont;
			}
			set {
				watermarkTextFont = value;
				//fontName = watermarkTextFont.ToString();
				//notify("WatermarkTextFontName");
				notify("WatermarkTextFont");
			}
		}

		/*private string fontName;
		/// <summary>
		/// User friendly font name to display on UI.
		/// </summary>
		public string WatermarkTextFontName {
			get {
				return this.fontName;
			}
		}*/

		private System.Drawing.Color watermarkTextColor;
		public System.Drawing.Color WatermarkTextColor {
			get {
				return watermarkTextColor;
			}
			set {
				if (((this.watermarkTextColor.A == value.A && this.watermarkTextColor.R == value.R) &&
				     this.watermarkTextColor.G == value.G) && this.watermarkTextColor.B == value.B) {
					return;
				}
				this.watermarkTextColor = value;
				notify("WatermarkTextColor");
			}
		}

		private StringAlignment watermarkTextAlignment;
		public StringAlignment WatermarkTextAlignment {
			get {
				return this.watermarkTextAlignment;
			}
			set {
				if (this.watermarkTextAlignment == value) {
					return;
				}
				this.watermarkTextAlignment = value;
				notify("WatermarkTextAlignment");
			}
		}
	}
}
