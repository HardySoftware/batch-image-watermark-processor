using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	class PathToImageConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return null;
			}

			if (value is string) {
				string path = (string)value;
				if (string.IsNullOrEmpty(path)) {
					return null;
				}

				value = new Uri(path, UriKind.RelativeOrAbsolute);
			}

			if (value is Uri) {
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.CacheOption = BitmapCacheOption.OnDemand;
				bi.UriSource = (Uri)value;
				bi.EndInit();
				return bi;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}