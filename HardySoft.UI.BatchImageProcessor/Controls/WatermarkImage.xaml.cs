using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using res = HardySoft.UI.BatchImageProcessor.Resources;
using System.Drawing;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for WatermarkImage.xaml
	/// </summary>
	public partial class WatermarkImage : UserControl {
		public WatermarkImage() {
			InitializeComponent();
		}

		public static DependencyProperty WatermarkImageFileProperty = DependencyProperty.Register("WatermarkImageFile",
			typeof(string), typeof(WatermarkImage));
		public string WatermarkImageFile {
			get {
				return (string)GetValue(WatermarkImageFileProperty);
			}
			set {
				SetValue(WatermarkImageFileProperty, value);
			}
		}

		public static DependencyProperty WatermarkImagePositionProperty = DependencyProperty.Register("WatermarkImagePosition",
			typeof(ContentAlignment), typeof(WatermarkImage));
		public ContentAlignment WatermarkImagePosition {
			get {
				return (ContentAlignment)GetValue(WatermarkImagePositionProperty);
			}
			set {
				SetValue(WatermarkImagePositionProperty, value);
			}
		}

		public static DependencyProperty WatermarkImageRotateAngleProperty = DependencyProperty.Register("WatermarkImageRotateAngle",
			typeof(int), typeof(WatermarkImage));
		public int WatermarkImageRotateAngle {
			get {
				return (int)GetValue(WatermarkImageRotateAngleProperty);
			}
			set {
				SetValue(WatermarkImageRotateAngleProperty, value);
			}
		}

		public static DependencyProperty WatermarkImageOpacityProperty = DependencyProperty.Register("WatermarkImageOpacity",
			typeof(double), typeof(WatermarkImage));
		public double WatermarkImageOpacity {
			get {
				return (double)GetValue(WatermarkImageOpacityProperty);
			}
			set {
				SetValue(WatermarkImageOpacityProperty, value);
			}
		}

		public static DependencyProperty WatermarkIndexProperty = DependencyProperty.Register("WatermarkIndex",
			typeof(int), typeof(WatermarkImage));
		public int WatermarkIndex {
			get {
				return (int)GetValue(WatermarkIndexProperty);
			}
			set {
				SetValue(WatermarkIndexProperty, value);
			}
		}

		public static new DependencyProperty PaddingProperty = DependencyProperty.Register("Padding",
			typeof(int), typeof(WatermarkImage));
		public new int Padding {
			get {
				return (int)GetValue(PaddingProperty);
			}
			set {
				SetValue(PaddingProperty, value);
			}
		}

		private void btnWatermarkImagePicker_Click(object sender, RoutedEventArgs e) {
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog {
				Filter = res.LanguageContent.
						Label_AllSupportedImagesFiles +
					" (*.jpg; *.jpeg; *.bmp; *.gif; *.png) |*.jpg;*.jpeg;*.bmp;*.gif;*.png",
				Title = res.LanguageContent.
					Label_OpenWatermarkImage
			};

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true) {
				// Open document
				this.WatermarkImageFile = dlg.FileName;
			}
		}

		private void btnRemoveWatermarkImage_Click(object sender, RoutedEventArgs e) {
			this.WatermarkImageFile = string.Empty;
		}
	}
}
