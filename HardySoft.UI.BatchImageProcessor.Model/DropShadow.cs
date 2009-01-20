using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;

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
			this.backgroundColor = Color.FromArgb(255, 0, 0, 0);
		}

		// System.Windows.Media.Color is not serializable, one workaround is to use other format to wrap around it.
		[NonSerialized]
		private Color backgroundColor;
		public Color BackgroundColor {
			get {
				return backgroundColor;
			}
			set {
				if (backgroundColor.A != value.A
					|| backgroundColor.R != value.R
					|| backgroundColor.G != value.G
					|| backgroundColor.B != value.B) {
					backgroundColor = value;
					notify("Background");
				}
			}
		}

		// for serialize purpose only, because System.Windows.Media.Color is not serializable
		public string BackgroundColorString {
			get {
				ColorConverter cnv = new ColorConverter();
				return cnv.ConvertToString(this.backgroundColor);
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentException("Color parameter cannot be null");
				}
				this.backgroundColor = (Color)ColorConverter.ConvertFromString(value);
			}
		}

		private int shadowDepth;
		public int ShadowDepth {
			get {
				return shadowDepth;
			}
			set {
				if (this.shadowDepth != value) {
					shadowDepth = value;
					notify("ShadowDepth");
				}
			}
		}

		private DropShadowLocation shadowLocation;
		public DropShadowLocation ShadowLocation {
			get {
				return shadowLocation;
			}
			set {
				if (this.shadowLocation != value) {
					shadowLocation = value;
					notify("ShadowLocation");
				}
			}
		}
	}
}
