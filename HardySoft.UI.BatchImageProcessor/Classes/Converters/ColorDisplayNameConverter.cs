using System;
using System.Windows.Data;
using System.Windows.Media;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class ColorDisplayNameConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value is Color) {
				Color c = (Color)value;

				return "Color Code: " + c.ToString();
			} else {
				return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}
	}
}
