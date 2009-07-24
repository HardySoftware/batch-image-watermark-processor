using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class DropShadow : INotifyPropertyChanged {
		//public event PropertyChangedEventHandler PropertyChanged;
		[NonSerialized]
		private PropertyChangedEventHandler propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged {
			add {
				propertyChanged += value;
			}
			remove {
				propertyChanged -= value;
			}
		}

		private void notify(string propName) {
			/*if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}*/

			if (propertyChanged != null) {
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		public DropShadow() {
			//this.backgroundColor = System.Drawing.Color.White;
			this.backgroundColor = System.Drawing.Color.FromArgb(255, 200, 200, 200);
			this.dropShadowColor = System.Drawing.Color.Black;
			this.shadowLocation = ContentAlignment.BottomRight;
		}

		// System.Windows.Media.Color is not serializable, one workaround is to use other format to wrap around it.
		// added investigation result:
		//      System.Windows.Media.Color could be Xml serialized
		//      try to use System.Drawing.Color + Converter to display color
		private System.Drawing.Color backgroundColor;
		public System.Drawing.Color BackgroundColor {
			get {
				return backgroundColor;
			}
			set {
				if (this.backgroundColor.A != value.A
					|| this.backgroundColor.R != value.R
					|| this.backgroundColor.G != value.G
					|| this.backgroundColor.B != value.B) {
					this.backgroundColor = value;
					notify("BackgroundColor");
				}
			}
		}

		private System.Drawing.Color dropShadowColor;
		public System.Drawing.Color DropShadowColor {
			get {
				return this.dropShadowColor;
			}
			set {
				if (this.dropShadowColor.A != value.A
					|| this.dropShadowColor.R != value.R
					|| this.dropShadowColor.G != value.G
					|| this.dropShadowColor.B != value.B) {
					this.dropShadowColor = value;
					notify("DropShadowColor");
				}
			}
		}

		private int shadowDepth;
		public int ShadowDepth {
			get {
				return this.shadowDepth;
			}
			set {
				if (this.shadowDepth != value) {
					this.shadowDepth = value;
					notify("ShadowDepth");
				}
			}
		}

		private ContentAlignment shadowLocation;
		public ContentAlignment ShadowLocation {
			get {
				return this.shadowLocation;
			}
			set {
				if (this.shadowLocation != value) {
					this.shadowLocation = value;
					notify("ShadowLocation");
				}
			}
		}
	}
}