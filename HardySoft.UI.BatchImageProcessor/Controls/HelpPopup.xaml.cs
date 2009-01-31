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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;
using System.Resources;

using HardySoft.UI.BatchImageProcessor.Resources;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for HelpPopup.xaml
	/// </summary>
	public partial class HelpPopup : Window {
		public HelpPopup() {
			InitializeComponent();
		}

		public void ShowDialog(double left, double top, string helpKey) {
			HelpPopupContent.Document = getRichText(HelpContent.ResourceManager.GetString(helpKey));
			
			this.Left = left;
			this.Top = top;
			this.ShowDialog();
		}

		private void btnClose_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private FlowDocument getRichText(string xaml) {
			MemoryStream memStream = new MemoryStream();
			byte[] data = Encoding.UTF8.GetBytes(xaml);
			memStream.Write(data, 0, data.Length);
			memStream.Position = 0;

			FlowDocument flowdoc = XamlReader.Load(memStream) as FlowDocument;

			return flowdoc;
		}
	}
}
