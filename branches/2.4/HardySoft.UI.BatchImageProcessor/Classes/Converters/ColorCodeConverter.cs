using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

using HardySoft.CC.Converter;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	/// <summary>
	/// Convert from System.Drawing.Color code to System.Windows.Media.Color
	/// </summary>
	public class ColorConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is System.Drawing.Color) {
				return HardySoft.CC.Converter.ColorConverter.ConvertColor((System.Drawing.Color)value);
			} else {
				// default color
				return System.Windows.Media.Colors.Black;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is System.Windows.Media.Color) {
				return HardySoft.CC.Converter.ColorConverter.ConvertColor((System.Windows.Media.Color)value);
			} else {
				// default color
				return System.Drawing.Color.Black;
			}
		}
	}
}
