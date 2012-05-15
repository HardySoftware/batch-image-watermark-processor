using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class TextSettingToRichtextConverter : IMultiValueConverter {
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			FlowDocument doc = new FlowDocument();
			Paragraph p = new Paragraph();
			Run r = new Run();

			for (int i = 0; i < values.Length; i++) {
				if (values[i] != null && values[i] is string) {
					r.Text = values[i].ToString();
				} else if (values[i] != null && values[i] is Font) {
					Font f = (Font)values[i];

					r.FontFamily = new System.Windows.Media.FontFamily(f.FontFamily.Name);
					r.FontSize = f.Size;
				} else if (values[i] != null && values[i] is System.Drawing.Color) {
					System.Drawing.Color textColor = (System.Drawing.Color)values[i];
					r.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
						textColor.A,
						textColor.R,
						textColor.G,
						textColor.B));
				} else if (values[i] != null && values[i] is StringAlignment) {
					StringAlignment alignment = (StringAlignment)values[i];
					switch (alignment) {
						case StringAlignment.Near:
							p.TextAlignment = System.Windows.TextAlignment.Left;
							break;
						case StringAlignment.Center:
							p.TextAlignment = System.Windows.TextAlignment.Center;
							break;
						case StringAlignment.Far:
							p.TextAlignment = System.Windows.TextAlignment.Right;
							break;
					}
				}
			}

			p.Inlines.Add(r);

			doc.Blocks.Add(p);

			return doc;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
