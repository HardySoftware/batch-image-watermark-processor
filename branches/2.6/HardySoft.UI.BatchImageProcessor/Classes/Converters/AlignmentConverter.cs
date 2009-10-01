using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class AlignmentConverter : IValueConverter {
		/// <summary>
		/// Convert value for binding from source object
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value != null) {
				if (value is StringAlignment) {
					StringAlignment alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), value.ToString());
					if (alignment == StringAlignment.Near
						&& string.Compare("Left", parameter.ToString(), true) == 0) {
						return true;
					} else if (alignment == StringAlignment.Center
					   && string.Compare("Center", parameter.ToString(), true) == 0) {
						return true;
					} else if (alignment == StringAlignment.Far
					   && string.Compare("Right", parameter.ToString(), true) == 0) {
						return true;
					} else {
						return false;
					}
				}
				return value;
			} else {
				return false;
			}
		}

		/// <summary>
		/// ConvertBack value from binding back to source object
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is bool) {
				bool isChecked = (bool)value;
				if (isChecked) {
					switch (parameter.ToString().ToLower()) {
						case "left":
							return StringAlignment.Near;
						case "center":
							return StringAlignment.Center;
						case "right":
							return StringAlignment.Far;
						default:
							return StringAlignment.Near;
					}
				} else {
					// default value
					return StringAlignment.Near;
				}
			} else {
				// default value
				return StringAlignment.Near;
			}
		}
	}
}
