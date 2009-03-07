using System;
using System.ComponentModel;
using System.Drawing;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class Watermark : INotifyPropertyChanged {
		public Watermark() {
			this.watermarkImagePosition = ContentAlignment.TopRight;
			this.watermarkTextPosition = ContentAlignment.BottomRight;
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
	}
}
