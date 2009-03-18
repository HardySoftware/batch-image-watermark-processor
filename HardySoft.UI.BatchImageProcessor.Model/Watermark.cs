using System;
using System.ComponentModel;
using System.Drawing;

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class Watermark : INotifyPropertyChanged {
		public Watermark() {
			this.watermarkImagePosition = ContentAlignment.TopRight;
			this.watermarkImageOpacity = 1.0;
			this.watermarkTextPosition = ContentAlignment.BottomRight;
			this.watermarkTextColor = System.Drawing.Color.FromArgb(153, 255, 255, 255);
			this.watermarkTextAlignment = StringAlignment.Center;
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		/*[NonSerialized]
		private PropertyChangedEventHandler propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged {
			add {
				propertyChanged += value;
			}
			remove {
				propertyChanged -= value;
			}
		}*/

		private void notify(string propName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}

			/*if (propertyChanged != null) {
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}*/
		}

		private string watermarkImageFile;
		public string WatermarkImageFile {
			get {
				return this.watermarkImageFile;
			}
			set {
				if (string.Compare(this.watermarkImageFile, value, true) != 0) {
					this.watermarkImageFile = value;
					notify("WatermarkImageFile");
				}
			}
		}

		private ContentAlignment watermarkImagePosition;
		public ContentAlignment WatermarkImagePosition {
			get {
				return this.watermarkImagePosition;
			}
			set {
				if (this.watermarkImagePosition != value) {
					watermarkImagePosition = value;
					notify("WatermarkImagePosition");
				}
			}
		}

		private int watermarkImageRotateAngle;
		[RangeValidator(0, RangeBoundaryType.Inclusive, 360, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_RotationAngle")]
		public int WatermarkImageRotateAngle {
			get {
				return watermarkImageRotateAngle;
			}
			set {
				if (this.watermarkImageRotateAngle != value) {
					this.watermarkImageRotateAngle = value;
					notify("WatermarkImageRotateAngle");
				}
			}
		}

		private double watermarkImageOpacity;
		[RangeValidator(0.0, RangeBoundaryType.Inclusive, 1.0, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_Opacity")]
		public double WatermarkImageOpacity {
			get {
				return watermarkImageOpacity;
			}
			set {
				if (this.watermarkImageOpacity != value) {
					this.watermarkImageOpacity = value;
					notify("WatermarkImageOpacity");
				}
			}
		}

		// TODO add feature to use EXIF information as watermark text
		private string watermarkText;
		public string WatermarkText {
			get {
				return this.watermarkText;
			}
			set {
				if (string.Compare(this.watermarkText, value, false) != 0) {
					this.watermarkText = value;
					notify("WatermarkText");
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
				fontName = watermarkTextFont.ToString();
				notify("WatermarkTextFontName");
				notify("WatermarkTextFont");
			}
		}

		private string fontName;
		/// <summary>
		/// User friendly font name to display on UI.
		/// </summary>
		public string WatermarkTextFontName {
			get {
				return this.fontName;
			}
		}

		private System.Drawing.Color watermarkTextColor;
		public System.Drawing.Color WatermarkTextColor {
			get {
				return watermarkTextColor;
			}
			set {
				if (this.watermarkTextColor.A != value.A
					|| this.watermarkTextColor.R != value.R
					|| this.watermarkTextColor.G != value.G
					|| this.watermarkTextColor.B != value.B) {
					this.watermarkTextColor = value;
					notify("WatermarkTextColor");
				}
			}
		}

		private ContentAlignment watermarkTextPosition;
		public ContentAlignment WatermarkTextPosition {
			get {
				return this.watermarkTextPosition;
			}
			set {
				if (this.watermarkTextPosition != value) {
					this.watermarkTextPosition = value;
					notify("WatermarkTextPosition");
				}
			}
		}

		private int watermarkTextRotateAngle;
		[RangeValidator(0, RangeBoundaryType.Inclusive, 360, RangeBoundaryType.Inclusive,
			MessageTemplate = "Validation_RotationAngle")]
		public int WatermarkTextRotateAngle {
			get {
				return this.watermarkTextRotateAngle;
			}
			set {
				if (this.watermarkTextRotateAngle != value) {
					this.watermarkTextRotateAngle = value;
					notify("WatermarkTextRotateAngle");
				}
			}
		}

		private StringAlignment watermarkTextAlignment;
		public StringAlignment WatermarkTextAlignment {
			get {
				return this.watermarkTextAlignment;
			}
			set {
				if (this.watermarkTextAlignment != value) {
					this.watermarkTextAlignment = value;
					notify("WatermarkTextAlignment");
				}
			}
		}
	}
}