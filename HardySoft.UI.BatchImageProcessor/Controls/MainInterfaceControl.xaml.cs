﻿using System;
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
using System.Windows.Forms;
using System.Drawing;

using Microsoft.Practices.Unity;

using HardySoft.CC;

using HardySoft.UI.BatchImageProcessor.View;
using HardySoft.UI.BatchImageProcessor.Presenter;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for MainInterfaceControl.xaml
	/// </summary>
	public partial class MainInterfaceControl : System.Windows.Controls.UserControl, IMainInterfaceControlView {
		private MainControl_Presenter presenter;
		private bool processing = false;

		public MainInterfaceControl() {
			IUnityContainer container = new UnityContainer();

			InitializeComponent();
		}

		[Dependency]
		public MainControl_Presenter Presenter {
			get {
				return presenter;
			}
			set {
				presenter = value;
				presenter.SetView(this);
			}
		}

		private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			// To get rid of selected item from image file list
			lvFileList.SelectedIndex = -1;
			e.Handled = true;
		}

		private void btnSourceDirectory_Click(object sender, RoutedEventArgs e) {
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			if (! string.IsNullOrEmpty(txtSourceDirectory.Text)) {
				dialog.SelectedPath  = txtSourceDirectory.Text;
			}
			dialog.Description = "Select Folder with Photos to Process";
			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK) {
				txtSourceDirectory.Text = dialog.SelectedPath;
			}
		}

		private void btnDestDirectory_Click(object sender, RoutedEventArgs e) {
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			if (!string.IsNullOrEmpty(txtDestDirectory.Text)) {
				dialog.SelectedPath = txtDestDirectory.Text;
			}
			dialog.Description = "Select Destination Folder";
			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK) {
				txtDestDirectory.Text = dialog.SelectedPath;
			}
		}

		private void btnWatermarkTextFontPicker_Click(object sender, RoutedEventArgs e) {
			FontDialog fd = new System.Windows.Forms.FontDialog();
			DialogResult dr = fd.ShowDialog();

			if (dr == System.Windows.Forms.DialogResult.OK) {
				//Apply your font name, size, styles, etc.
				ps.Watermark.WatermarkTextFont = fd.Font;
			}
		}

		private void btnWatermarkImagePicker_Click(object sender, RoutedEventArgs e) {
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			//dlg.DefaultExt = ".txt"; // Default file extension
			dlg.Filter = "All Supported Images Files (*.jpg; *.jpeg; *.bmp; *.gif) |*.jpg;*.jpeg;*.bmp;*.gif"; // Filter files by extension
			dlg.Title = "Open Watermark Image";

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true) {
				// Open document
				ps.Watermark.WatermarkImageFile = dlg.FileName;
			}
		}

		private void btnRemoveWatermarkImage_Click(object sender, RoutedEventArgs e) {
			ps.Watermark.WatermarkImageFile = string.Empty;
		}

		private void btnShadowBackgroundPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = ps.DropShadowSetting.Background;
			//cPicker.Owner = this.Parent;

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				ps.DropShadowSetting.Background = cPicker.SelectedColor;
			}
		}

		private void btnImageBorderColorPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = ps.DropShadowSetting.Background;
			//cPicker.Owner = this.Parent;

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				ps.BorderSetting.BorderColor = cPicker.SelectedColor;
			}
		}

		#region View member
		private ProjectSetting ps;

		public ProjectSetting PS {
			get {
				return this.ps;
			}
			set {
				ps = value;

				tabConfiguration.IsEnabled = ps.ProjectCreated;

				ps.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ps_PropertyChanged);
				ps.Watermark.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Watermark_PropertyChanged);

				tabConfiguration.DataContext = this.ps;
			}
		}

		void Watermark_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			/*if (e.PropertyName == "WatermarkImageFile") {
				if (string.IsNullOrEmpty(ps.Watermark.WatermarkImageFile)) {
					imgWatermarkImage.Source = null;
				} else {
					imgWatermarkImage.Source = new BitmapImage(new Uri(ps.Watermark.WatermarkImageFile, UriKind.RelativeOrAbsolute));
				}
			}*/
		}

		void ps_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			// if any value of the object is changed.
			//this.isDirty = ps.IsDirty;
		}

		public Exception ErrorMessage {
			set {
				string messageBoxText = value.ToString();
				string caption = "Error";
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;

				// Display message box
				System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
			}
		}

		public List<string> ErrorMessages {
			set {
				string messageBoxText = string.Empty;
				foreach (string s in value) {
					messageBoxText += Properties.Resources.ResourceManager.GetString(s) + "\r\n";
				}
				string caption = "Error";
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;

				// Display message box
				System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
			}
		}
		#endregion

		#region View events
		public event EventHandler NewProjectCreated;
		public event ProjectWithFileNameEventHandler SaveProject;
		public event ProjectWithFileNameEventHandler SaveProjectAs;
		public event ProjectWithFileNameEventHandler OpenProject;
		public event ProcessThreadNumberEventHandler ProcessImage;
		public event EventHandler StopProcessing;
		#endregion

		#region Commands
		#region New Command
		private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			EventHandler handlers = NewProjectCreated;
			if (handlers != null) {
				handlers(this, EventArgs.Empty);
			}
		}
		#endregion

		#region Open Command
		private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}
		
		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "All Image Process Project Files (*.hsbip)|*.hsbip;";
			openFile.Multiselect = false;
			if (openFile.ShowDialog() == DialogResult.OK) {
				string projectFileName = openFile.FileName;
				if (!string.IsNullOrEmpty(projectFileName)) {
					ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(projectFileName);

					ProjectWithFileNameEventHandler handlers = OpenProject;
					if (handlers != null) {
						handlers(this, args);
					}
				}
			}
		}
		#endregion

		#region Save Command
		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			//e.CanExecute = this.isDirty;
			if (ps == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = ps.IsDirty;
			}
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "All Image Process Project Files (*.hsbip)|*.hsbip;";
			if (saveFile.ShowDialog() == DialogResult.OK) {
				string projectFileName = saveFile.FileName;
				if (! string.IsNullOrEmpty(projectFileName)) {
					ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(projectFileName);

					ProjectWithFileNameEventHandler handlers = SaveProject;
					if (handlers != null) {
						handlers(this, args);
					}
				}
			}
		}
		#endregion

		#region Save As Command
		private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (ps == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = ps.ProjectCreated;
			}
		}
		
		private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "All Image Process Project Files (*.hsbip)|*.hsbip;";
			if (saveFile.ShowDialog() == DialogResult.OK) {
				string projectFileName = saveFile.FileName;
				if (!string.IsNullOrEmpty(projectFileName)) {
					ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(projectFileName);

					ProjectWithFileNameEventHandler handlers = SaveProjectAs;
					if (handlers != null) {
						handlers(this, args);
					}
				}
			}
		}
		#endregion

		#region Stop Command
		private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = this.processing;
		}
		
		private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}
		#endregion

		#region Make Command
		private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (ps == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = ps.ProjectCreated;
			}
		}

		private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			ProcessThreadNumberEventHandler handlers = ProcessImage;
			if (handlers != null) {
				ProcessThreadNumberEventArgs args = new ProcessThreadNumberEventArgs(Properties.Settings.Default.ThreadNumber);
				handlers(this, args);
			}
		}
		#endregion

		#region Exit Command
		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}
		
		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			// TODO detect working threads status and gracefully shut them down
			System.Windows.Application.Current.Shutdown();
		}
		#endregion

		#region Preference window command
		private void PreferenceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void PreferenceCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			IUnityContainer container = new UnityContainer();

			Preference preferenceWindow = (Preference)container.Resolve<Preference>();
			preferenceWindow.ShowDialog();
		}
		#endregion

		#region About window command
		private void AboutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			About about = new About();
			about.ShowDialog();
		}
		#endregion
		#endregion
	}
}