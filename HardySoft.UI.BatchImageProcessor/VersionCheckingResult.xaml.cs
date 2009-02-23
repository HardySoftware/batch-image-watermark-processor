using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for VersionCheckingResult.xaml
	/// </summary>
	public partial class VersionCheckingResult : Window {
		public VersionCheckingResult() {
			InitializeComponent();

			this.mainGrid.DataContext = this;
		}

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			this.DragMove();
		}

		static VersionCheckingResult() {
			LatestVersionProperty = DependencyProperty.Register("LatestVersion",
				typeof(Version), typeof(VersionCheckingResult));

			MyVersionProperty = DependencyProperty.Register("MyVersion",
				typeof(Version), typeof(VersionCheckingResult));

			VersionCompareStatusProperty = DependencyProperty.Register("VersionCompareStatus",
				typeof(string), typeof(VersionCheckingResult));

			ApplicationURLProperty = DependencyProperty.Register("ProductURL",
				typeof(string), typeof(VersionCheckingResult));
		}

		public static readonly DependencyProperty LatestVersionProperty;
		public static readonly DependencyProperty MyVersionProperty;
		public static readonly DependencyProperty VersionCompareStatusProperty;
		public static readonly DependencyProperty ApplicationURLProperty;

		public Version LatestVersion {
			get {
				return (Version)GetValue(LatestVersionProperty);
			}
			set {
				SetValue(LatestVersionProperty, value);
			}
		}

		public Version MyVersion {
			get {
				return (Version)GetValue(MyVersionProperty);
			}
			set {
				SetValue(MyVersionProperty, value);
			}
		}

		public string VersionCompareStatus {
			get {
				return (string)GetValue(VersionCompareStatusProperty);
			}
			set {
				SetValue(VersionCompareStatusProperty, value);
			}
		}

		public string ApplicationURL {
			get {
				return (string)GetValue(ApplicationURLProperty);
			}
			set {
				SetValue(ApplicationURLProperty, value);
			}
		}

		private void btnOK_Click(object sender, RoutedEventArgs e) {
			this.Close();
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
