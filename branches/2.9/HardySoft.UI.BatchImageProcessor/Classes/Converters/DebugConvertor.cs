using System;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class DebugConvertor : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value; // Add the breakpoint here!!
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return value; // Add the breakpoint here!!
		}
	}
}
