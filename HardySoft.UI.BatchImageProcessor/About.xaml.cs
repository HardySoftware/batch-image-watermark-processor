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
using System.Diagnostics;

using HardySoft.UI.BatchImageProcessor.Classes;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : Window {
		public About() {
			InitializeComponent();

			AssembyInformation ai = new AssembyInformation();
			InformationSection.DataContext = ai;
		}

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			this.DragMove();
		}

		private void lvAssemblyList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			// To get rid of selected item from the list
			lvAssemblyList.SelectedIndex = -1;
			e.Handled = true;
		}

		private void btnOK_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}

		private void hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			string navigateUri = e.Uri.ToString();
			// if the URI somehow came from an untrusted source, make sure to
			// validate it before calling Process.Start(), e.g. check to see
			// the scheme is HTTP, etc.
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}
	}
}
