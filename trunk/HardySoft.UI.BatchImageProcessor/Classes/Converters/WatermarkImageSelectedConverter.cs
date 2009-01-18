using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class WatermarkImageSelectedConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return false;
			}

			double actualWidth = 0;
			if (value is double) {
				actualWidth = (double)value;
			}

			if (actualWidth > 0) {
				return true;
			} else {
				return false;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
