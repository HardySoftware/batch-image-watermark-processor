using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;

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
			string helpString = HelpContent.ResourceManager.GetString(helpKey, Thread.CurrentThread.CurrentCulture);
			if (!string.IsNullOrEmpty(helpString)) {
				FlowDocument flowDoc = getRichText(helpString);
				if (flowDoc != null) {
					HelpPopupContent.Document = flowDoc;

					this.Left = left;
					this.Top = top;
					this.ShowDialog();
				}
			}
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
