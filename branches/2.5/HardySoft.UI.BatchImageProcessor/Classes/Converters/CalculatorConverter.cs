using System;
using System.Globalization;
using System.Windows.Data;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class CalculatorConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is double) {
				double valueA = double.Parse(value.ToString());
				double valueB = double.Parse(parameter.ToString());
				return valueA * valueB;
			} else {
				return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
