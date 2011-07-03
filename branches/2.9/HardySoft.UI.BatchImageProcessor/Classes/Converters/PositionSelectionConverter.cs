using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class PositionSelectionConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ContentAlignment) {
				value = (ContentAlignment)value;

				if (value.ToString() == parameter.ToString()) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			bool selected = (bool)value;

			if (selected) {
				ContentAlignment alignment = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), parameter.ToString());
				return alignment;
			} else {
				return null;
			}
		}
	}
}
