using System;
using System.Windows.Data;
using System.Drawing;
using System.Globalization;

using res = HardySoft.UI.BatchImageProcessor.Resources;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class ColorDisplayNameConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is Color) {
				Color c = (Color)value;

				char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

				byte[] bytes = new byte[4];
				bytes[0] = c.A;
				bytes[1] = c.R;
				bytes[2] = c.G;
				bytes[3] = c.B;
				char[] chars = new char[bytes.Length * 2];
				for (int i = 0; i < bytes.Length; i++) {
					int b = bytes[i];
					chars[i * 2] = hexDigits[b >> 4];
					chars[i * 2 + 1] = hexDigits[b & 0xF];
				}
				string hexCode = new string(chars);

				return res.LanguageContent.Label_ColorCode + " #" + hexCode;
			} else {
				return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}
	}
}
