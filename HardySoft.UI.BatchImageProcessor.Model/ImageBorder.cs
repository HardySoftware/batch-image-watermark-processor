﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class ImageBorder : INotifyPropertyChanged {
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

		public ImageBorder() {
			this.borderColor = Color.FromArgb(255, 0, 0, 0);
		}

		private int borderWidth;
		public int BorderWidth {
			get {
				return borderWidth;
			}
			set {
				if (this.borderWidth != value) {
					borderWidth = value;
					notify("BorderWidth");
				}
			}
		}

		[NonSerialized]
		private Color borderColor;
		public Color BorderColor {
			get {
				return borderColor;
			}
			set {
				if (borderColor.A != value.A
					|| borderColor.R != value.R
					|| borderColor.G != value.G
					|| borderColor.B != value.B) {
					borderColor = value;
					notify("BorderColor");
				}
			}
		}

		public string BorderColorString {
			get {
				ColorConverter cnv = new ColorConverter();
				return cnv.ConvertToString(this.borderColor);
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentException("Color parameter cannot be null");
				}
				this.borderColor = (Color)ColorConverter.ConvertFromString(value);
			}
		}
	}
}