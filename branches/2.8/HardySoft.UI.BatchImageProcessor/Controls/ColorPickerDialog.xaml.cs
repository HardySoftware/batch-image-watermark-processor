using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for ColorPickerDialog.xaml
	/// </summary>
	public partial class ColorPickerDialog : Window {
		public ColorPickerDialog() {
			InitializeComponent();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e) {
			btnOK.IsEnabled = false;
			m_color = cPicker.SelectedColor;
			DialogResult = true;
			Hide();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			btnOK.IsEnabled = false;
			DialogResult = false;
		}

		private void onSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e) {
			if (e.NewValue != m_color) {
				btnOK.IsEnabled = true;
			}
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			btnOK.IsEnabled = false;
			base.OnClosing(e);
		}

		private Color m_color = new Color();
		private Color startingColor = new Color();

		public Color SelectedColor {
			get {
				return m_color;
			}
		}

		public Color StartingColor {
			get {
				return startingColor;
			}
			set {
				cPicker.SelectedColor = value;
				btnOK.IsEnabled = false;
			}
		}

		private void tbTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			this.DragMove();
		}
	}
}
