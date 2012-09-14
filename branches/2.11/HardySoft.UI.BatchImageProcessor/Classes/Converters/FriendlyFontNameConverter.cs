using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class FriendlyFontNameConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is Font) {
				Font font = value as Font;
				return font.ToString();
			} else {
				return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException("This method should never be called");
		}
	}
}
