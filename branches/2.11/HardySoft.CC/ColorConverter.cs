using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.CC.Converter {
	public class ColorConverter {
		public static System.Drawing.Color ConvertColor(System.Windows.Media.Color color) {
			System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color.A,
				color.R,
				color.G,
				color.B);

			return newColor;
		}

		public static System.Windows.Media.Color ConvertColor(System.Drawing.Color color) {
			System.Windows.Media.Color newColor = new System.Windows.Media.Color();
			newColor.A = color.A;
			newColor.R = color.R;
			newColor.G = color.G;
			newColor.B = color.B;

			return newColor;
		}
	}
}
