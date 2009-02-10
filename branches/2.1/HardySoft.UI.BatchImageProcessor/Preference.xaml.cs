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

using Microsoft.Practices.Unity;

using HardySoft.UI.BatchImageProcessor.View;
using HardySoft.UI.BatchImageProcessor.Presenter;
using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.Controls;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for Preference.xaml
	/// </summary>
	public partial class Preference : Window, IPreference {
		private PreferenceWindow_Presenter presenter;

		public Preference() {
			InitializeComponent();

			//cmbAppSkin.DataContext = this;
			//sThreadNumber.DataContext = this;
			//tbThreadNumber.DataContext = this;
			InformationSection.DataContext = this;
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
		private uint threadNumber = Properties.Settings.Default.ThreadNumber;
		public uint ThreadNumber {
			get {
				return threadNumber;
			}
			set {
				if (value > 0) {
					threadNumber = value;
				}
			}
		}

		private Skin applicationSkin = Properties.Settings.Default.AppSkin;
		public Skin ApplicationSkin {
			get {
				return applicationSkin;
			}
			set {
				applicationSkin = value;
			}
		}
		#endregion

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			this.DragMove();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			Properties.Settings.Default.AppSkin = this.applicationSkin;
			Properties.Settings.Default.ThreadNumber = this.threadNumber;
			Properties.Settings.Default.Save();

			DialogResult = false;
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
