using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class CornerOnlyToVisibilityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is bool) {
				if (! (bool)value) {
					return Visibility.Visible;
				} else {
					return Visibility.Hidden;
				}
			} else {
				return Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			Visibility visibilityValue = (Visibility)Enum.Parse(typeof(Visibility), value.ToString());

			switch (visibilityValue) {
				case Visibility.Visible:
					return false;
				default:
					return true;
			}
		}
	}
}
