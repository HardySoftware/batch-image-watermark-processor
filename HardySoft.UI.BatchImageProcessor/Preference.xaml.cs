using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using HardySoft.UI.BatchImageProcessor.Classes;
using HardySoft.UI.BatchImageProcessor.Controls;
using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.Presenter;
using HardySoft.UI.BatchImageProcessor.View;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for Preference.xaml
	/// </summary>
	public partial class Preference : Window, IPreferenceView {
		private PreferenceWindow_Presenter presenter;

		public Preference() {
			InitializeComponent();

			//cmbAppSkin.DataContext = this;
			//sThreadNumber.DataContext = this;
			//tbThreadNumber.DataContext = this;
			InformationSection.DataContext = this;

			Utilities.SetSkin();
		}

		[Dependency]
		public PreferenceWindow_Presenter Presenter {
			get {
				return presenter;
			}
			set {
				presenter = value;
				presenter.SetView(this);
			}
		}

		#region View member
		private Skin applicationSkin = Properties.Settings.Default.AppSkin;
		public Skin ApplicationSkin {
			get {
				return applicationSkin;
			}
			set {
				applicationSkin = value;
			}
		}

		private string dateTimeFormatString = Properties.Settings.Default.DateTimeFormatString;
		public string DateTimeFormatString {
			get {
				return CheckDateTimeFormatString(this.dateTimeFormatString);
			}
			set {
				this.dateTimeFormatString = CheckDateTimeFormatString(value);
			}
		}

		private string CheckDateTimeFormatString(string format) {
			foreach (KeyValuePair<string, string> validDateTimeFormatString in this.ValidDateTimeFormatStrings) {
				if (string.Compare(format, validDateTimeFormatString.Key, false) == 0) {
					return format;
				}
			}

			// this is the default value.
			return "d";
		}

		private Dictionary<string, string> validDateTimeFormatStrings;
		public Dictionary<string, string> ValidDateTimeFormatStrings {
			get {
				return this.validDateTimeFormatStrings;
			}
			set {
				this.validDateTimeFormatStrings = value;
				DateTime d = DateTime.Now;

				Dictionary<string, string> items = new Dictionary<string, string>();
				foreach (KeyValuePair<string, string> formatString in value) {
					string displayValue = HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.ResourceManager.GetString(formatString.Value,
							Thread.CurrentThread.CurrentCulture);
					displayValue = displayValue + " (" + d.ToString(formatString.Key) + ")";
					//this.validDateTimeFormatStrings.Add(formatString.Key, displayValue);
					items.Add(formatString.Key, displayValue);
				}

				//cmbDateTimeFormat.ItemsSource = this.validDateTimeFormatStrings;
				cmbDateTimeFormat.ItemsSource = items;
			}
		}
		#endregion

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			this.DragMove();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			Properties.Settings.Default.AppSkin = this.applicationSkin;
			Properties.Settings.Default.DateTimeFormatString = this.dateTimeFormatString;
			Properties.Settings.Default.Save();

			DialogResult = false;

			MainWindow mainWindow = this.Owner as MainWindow;
			if (mainWindow != null) {
				mainWindow.SetSkin();
			}
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
		}

		private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			HelpPopup popup = new HelpPopup();

			// Mouse position
			System.Windows.Point mousePoint = this.PointToScreen(Mouse.GetPosition(this));
			//System.Windows.Point mousePoint = Mouse.GetPosition(this);
			popup.Owner = this;
			popup.ShowDialog(mousePoint.X, mousePoint.Y, (string)e.Parameter);
		}
	}
}