using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class PathToThumbConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return null;
			}

			if (value is string) {
				string path = (string)value;
				if (string.IsNullOrEmpty(path) || ! File.Exists(path)) {
					return null;
				}

				value = new Uri(path, UriKind.RelativeOrAbsolute);
			}

			if (value is Uri) {
				Uri imageUri = (Uri)value;
				// if file name has space(s) in file name AbsolutePath quits working for File.Exists
				// if (!File.Exists(imageUri.AbsolutePath)) {
				//	return null;
				// }
				if (!File.Exists(imageUri.LocalPath)) {
					return null;
				}
				
				MemoryStream ms = new MemoryStream();
				byte[] bytArray = File.ReadAllBytes(imageUri.LocalPath);
				ms.Write(bytArray, 0, bytArray.Length);
				ms.Position = 0;

				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.CacheOption = BitmapCacheOption.OnDemand;
				bi.DecodePixelWidth = 200;
				//bi.DecodePixelHeight = 60;
				//bi.UriSource = imageUri;
				bi.StreamSource = ms;
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
