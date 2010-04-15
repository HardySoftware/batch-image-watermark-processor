using System;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class NotOperationConverter : IValueConverter {
		/// <summary>
		/// Convert value for binding from source object
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is bool) {
				bool myValue = (bool)value;
				return !myValue;
			} else {
				return false;
			}
		}

		/// <summary>
		/// ConvertBack value from binding back to source object
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new Exception("Cant convert back");
		}
	}
}
