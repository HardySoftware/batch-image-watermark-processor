using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenXMLWriter {
	/// <summary>
	/// Interaction logic for XamlView.xaml
	/// </summary>
	public partial class XamlView : Window {
		public XamlView() {
			InitializeComponent();
		}

		public string XamlString {
			/*get {
				return 
			}*/
			set {
				tbXaml.Text = value;
			}
		}

		private void btnClose_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
