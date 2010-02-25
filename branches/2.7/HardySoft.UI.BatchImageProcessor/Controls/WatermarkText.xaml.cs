using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for WatermarkText.xaml
	/// </summary>
	public partial class WatermarkText : System.Windows.Controls.UserControl {
		public WatermarkText(Dictionary<string, string> translatedExifTags) {
			InitializeComponent();

			cmbExifTag.ItemsSource = translatedExifTags;
			cmbExifTag.SelectedIndex = 0;
		}

		public static DependencyProperty WatermarkTextToDisplayProperty = DependencyProperty.Register("WatermarkTextToDisplay",
			typeof(string), typeof(WatermarkText));
		public string WatermarkTextToDisplay {
			get {
				return (string)GetValue(WatermarkTextToDisplayProperty);
			}
			set {
				SetValue(WatermarkTextToDisplayProperty, value);
			}
		}

		public static DependencyProperty WatermarkTextFontProperty = DependencyProperty.Register("WatermarkTextFont",
			typeof(Font), typeof(WatermarkText));
		public Font WatermarkTextFont {
			get {
				return (Font)GetValue(WatermarkTextFontProperty);
			}
			set {
				SetValue(WatermarkTextFontProperty, value);
			}
		}

		public static DependencyProperty WatermarkTextColorProperty = DependencyProperty.Register("WatermarkTextColor",
			typeof(System.Drawing.Color), typeof(WatermarkText));
		public System.Drawing.Color WatermarkTextColor {
			get {
				return (System.Drawing.Color)GetValue(WatermarkTextColorProperty);
			}
			set {
				SetValue(WatermarkTextColorProperty, value);
			}
		}

		public static DependencyProperty WatermarkTextPositionProperty = DependencyProperty.Register("WatermarkPosition",
			typeof(ContentAlignment), typeof(WatermarkText));
		public ContentAlignment WatermarkPosition {
			get {
				return (ContentAlignment)GetValue(WatermarkTextPositionProperty);
			}
			set {
				SetValue(WatermarkTextPositionProperty, value);
			}
		}

		public static DependencyProperty WatermarkTextRotateAngleProperty = DependencyProperty.Register("WatermarkTextRotateAngle",
			typeof(int), typeof(WatermarkText));
		public int WatermarkTextRotateAngle {
			get {
				return (int)GetValue(WatermarkTextRotateAngleProperty);
			}
			set {
				SetValue(WatermarkTextRotateAngleProperty, value);
			}
		}

		public static DependencyProperty WatermarkTextAlignmentProperty = DependencyProperty.Register("WatermarkTextAlignment",
			typeof(StringAlignment), typeof(WatermarkText));
		public StringAlignment WatermarkTextAlignment {
			get {
				return (StringAlignment)GetValue(WatermarkTextAlignmentProperty);
			}
			set {
				SetValue(WatermarkTextAlignmentProperty, value);
			}
		}

		public static DependencyProperty ExifTagProperty = DependencyProperty.Register("ExifTag",
			typeof(Dictionary<string, string>), typeof(WatermarkText));
		public Dictionary<string, string> ExifTag {
			get {
				return (Dictionary<string, string>)GetValue(WatermarkTextAlignmentProperty);
			}
			set {
				SetValue(WatermarkTextAlignmentProperty, value);
			}
		}

		public static DependencyProperty WatermarkIndexProperty = DependencyProperty.Register("WatermarkIndex",
			typeof(int), typeof(WatermarkText));
		public int WatermarkIndex {
			get {
				return (int)GetValue(WatermarkIndexProperty);
			}
			set {
				SetValue(WatermarkIndexProperty, value);
			}
		}

		public static DependencyProperty PaddingProperty = DependencyProperty.Register("Padding",
			typeof(int), typeof(WatermarkText));
		public int Padding {
			get {
				return (int)GetValue(PaddingProperty);
			}
			set {
				SetValue(PaddingProperty, value);
			}
		}

		private void btnWatermarkTextFontPicker_Click(object sender, RoutedEventArgs e) {
			FontDialog fd = new System.Windows.Forms.FontDialog();
			if (WatermarkTextFont != null) {
				fd.Font = WatermarkTextFont;
			}
			DialogResult dr = fd.ShowDialog();

			if (dr == System.Windows.Forms.DialogResult.OK) {
				//Apply your font name, size, styles, etc.
				WatermarkTextFont = fd.Font;
			}
		}

		private void btnWatermarkTextColorPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = HardySoft.CC.Converter.ColorConverter.ConvertColor(this.WatermarkTextColor);
			cPicker.Owner = Window.GetWindow(this);

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				this.WatermarkTextColor = HardySoft.CC.Converter.ColorConverter.ConvertColor(cPicker.SelectedColor);
			}
		}

		private void btnInsertExifTag_Click(object sender, RoutedEventArgs e) {
			if (cmbExifTag.SelectedIndex <= 0) {
				return;
			}

			KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>)cmbExifTag.SelectedValue;
			string tag = "[[" + selectedItem.Key + "]]";

			int insertPosition = txtWatermarkText.CaretIndex;
			string firstPart = txtWatermarkText.Text.Substring(0, insertPosition);
			string secondPart = txtWatermarkText.Text.Substring(insertPosition);

			this.WatermarkTextToDisplay = firstPart + tag + secondPart;

			txtWatermarkText.Text = WatermarkTextToDisplay;
			txtWatermarkText.CaretIndex = firstPart.Length + tag.Length;
		}
	}
}
