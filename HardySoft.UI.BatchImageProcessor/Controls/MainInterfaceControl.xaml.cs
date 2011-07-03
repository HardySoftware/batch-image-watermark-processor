using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

using HardySoft.CC.Converter;

using res = HardySoft.UI.BatchImageProcessor.Resources;
using HardySoft.UI.BatchImageProcessor.Classes;
using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.Presenter;
using HardySoft.UI.BatchImageProcessor.View;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Controls {
	/// <summary>
	/// Interaction logic for MainInterfaceControl.xaml
	/// </summary>
	public partial class MainInterfaceControl : System.Windows.Controls.UserControl, IMainInterfaceControlView {
		// TODO add more option to "Output" tab to select file format for processed files. Something like batch convert..
		// TODO in image file list add assembly new column to include button to remove image from list
		// TODO convert "image effect" into add-ins and open programming interface
		// TODO Think about "Glass Behavior" @ http://jmorrill.hjtcentral.com/Home/tabid/428/EntryId/403/Glass-Behavior-for-WPF.aspx
		private MainControl_Presenter presenter;
		private DispatcherTimer dispatcherTimer;
		private IUnityContainer container = new UnityContainer();

		public MainInterfaceControl() {
			InitializeComponent();

			// allow drag-n-drop from File Explorer
			this.AllowDrop = true;

			//  DispatcherTimer setup to enforce commands to check can execute state
			dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			dispatcherTimer.Start();

			res.LanguageContent.Culture = Thread.CurrentThread.CurrentCulture;
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

		public IConfiguration HiddenConfig {
			get;
			set;
		}

		public void NewProject() {
			newProject();
		}

		public event ProjectFileNameObtainedHandler ProjectFileNameObtained;

		protected void OnProjectFileNameObtained() {
			if (ProjectFileNameObtained != null && !string.IsNullOrEmpty(this.presenter.CurrentProjectFile)) {
				ProjectFileNameEventArgs args = new ProjectFileNameEventArgs(this.presenter.CurrentProjectFile,
					this.ps.IsDirty);
				ProjectFileNameObtained(this, args);
			}
		}

		#region UI Event Handler
		void dispatcherTimer_Tick(object sender, EventArgs e) {
			// force commands to re-evaluate
			CommandManager.InvalidateRequerySuggested();
		}

		private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			// To get rid of selected item from image file list
			lvFileList.SelectedIndex = -1;
			e.Handled = true;
		}

		private void btnSourceDirectory_Click(object sender, RoutedEventArgs e) {
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			if (!string.IsNullOrEmpty(txtSourceDirectory.Text)) {
				dialog.SelectedPath = txtSourceDirectory.Text;
			}
			dialog.Description = res.LanguageContent.Label_SourceFolder;
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
			dialog.Description = res.LanguageContent.Label_DestFolder;
			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK) {
				txtDestDirectory.Text = dialog.SelectedPath;
			}
		}

		private void btnShadowBackgroundPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = ColorConverter.ConvertColor(ps.DropShadowSetting.BackgroundColor);
			cPicker.Owner = Window.GetWindow(this);

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				ps.DropShadowSetting.BackgroundColor = ColorConverter.ConvertColor(cPicker.SelectedColor);
			}
		}

		private void btnShadowColorPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = ColorConverter.ConvertColor(ps.DropShadowSetting.DropShadowColor);
			cPicker.Owner = Window.GetWindow(this);

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				ps.DropShadowSetting.DropShadowColor = ColorConverter.ConvertColor(cPicker.SelectedColor);
			}
		}

		private void btnImageBorderColorPicker_Click(object sender, RoutedEventArgs e) {
			ColorPickerDialog cPicker = new ColorPickerDialog();
			cPicker.StartingColor = ColorConverter.ConvertColor(ps.BorderSetting.BorderColor);
			cPicker.Owner = Window.GetWindow(this);

			bool? dialogResult = cPicker.ShowDialog();
			if (dialogResult != null && (bool)dialogResult == true) {
				ps.BorderSetting.BorderColor = ColorConverter.ConvertColor(cPicker.SelectedColor);
			}
		}

		private void lbWatermarkCollection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.AddedItems.Count > 0) {
#if DEBUG

				System.Diagnostics.Debug.WriteLine("watermark selection changed " + e.AddedItems[0].ToString() + " at " + DateTime.Now);
#endif
				WatermarkSelection selectedItem = e.AddedItems[0] as WatermarkSelection;

				switch (selectedItem.WatermarkType) {
					case "Image":
						presenter.CreateWatermarkImageContent(selectedItem.Index);
						break;
					case "Text":
						presenter.CreateWatermarkTextContent(selectedItem.Index);
						break;
				}
			}
		}

		#region Drag-Drop event handlers
		protected override void OnPreviewDragOver(System.Windows.DragEventArgs e) {
			// we only want to deal with assembly single file.
			if (isSingleFile(e) != null) {
				e.Effects = System.Windows.DragDropEffects.Copy;
			} else {
				e.Effects = System.Windows.DragDropEffects.None;
			}

			// Mark the event as handled
			e.Handled = true;

			base.OnPreviewDragOver(e);
		}

		protected override void OnPreviewDrop(System.Windows.DragEventArgs e) {
			// Mark the event as handled, so TextBox's native Drop handler is not called.
			e.Handled = true;

			string fileName = isSingleFile(e);
			if (fileName == null) {
				return;
			}

			//System.Windows.MessageBox.Show(fileName);
			FileInfo fi = new FileInfo(fileName);
			if (string.Compare(fi.Extension, ".hsbip", true) == 0) {
				// TODO add support to drag-n-drop supported image files into projects.
				// only handle supported format
				if (this.ps.IsDirty) {
					MessageBoxResult result = System.Windows.MessageBox.Show(res.LanguageContent.Message_SavePrompt,
						res.LanguageContent.Label_UnsavedProject,
						MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

					switch (result) {
						case MessageBoxResult.Yes:
							saveProject();
							openProject(fileName);
							break;
						case MessageBoxResult.No:
							openProject(fileName);
							break;
						default:
							// "Cancel" do nothing
							break;
					}
				} else {
					openProject(fileName);
				}
			}

			base.OnPreviewDrop(e);
		}

		// If the data object in args is assembly single file, this method will return the filename.
		// Otherwise, it returns null.
		private string isSingleFile(System.Windows.DragEventArgs args) {
			// Check for files in the hovering data object.
			if (args.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true)) {
				string[] fileNames = args.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
				// Check fo assembly single file or folder.
				if (fileNames.Length == 1) {
					// Check for assembly file (assembly directory will return false).
					if (File.Exists(fileNames[0])) {
						// At this point we know there is assembly single file.
						return fileNames[0];
					}
				}
			}
			return null;
		}
		#endregion
		#endregion

		#region View member
		private ProjectSetting ps;

		public ProjectSetting PS {
			get {
				return this.ps;
			}
			set {
				ps = value;

				tabConfiguration.IsEnabled = ps.ProjectCreated;

				ps.PropertyChanged -= new PropertyChangedEventHandler(ps_PropertyChanged);
				ps.PropertyChanged += new PropertyChangedEventHandler(ps_PropertyChanged);

				tabConfiguration.DataContext = this.ps;
			}
		}

		public Dictionary<string, string> ExifTag {
			set {
				Dictionary<string, string> translatedExifTags = new Dictionary<string, string>();
				translatedExifTags.Add("", res.LanguageContent.Enum_None);
				foreach (KeyValuePair<string, string> item in value) {
					//item.Value = 
					string displayName = res.LanguageContent.ResourceManager.GetString(item.Value,
						Thread.CurrentThread.CurrentCulture);
					translatedExifTags.Add(item.Key, displayName);
				}

				container.RegisterInstance<Dictionary<string, string>>(translatedExifTags);
			}
		}

		public List<ExifContainerItem> ExifContainer {
			get {
#if DEBUG
				System.Diagnostics.Debug.WriteLine("Current Thread: "
					+ Thread.CurrentThread.ManagedThreadId
					+ " Culture: "
					+ Thread.CurrentThread.CurrentCulture.ToString()
					+ " in main UI.");
#endif
				return Utilities.GetExifContainer(true);
			}
		}

		void ps_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			// if any value of the object is changed.
			OnProjectFileNameObtained();
		}

		public Exception ErrorMessage {
			set {
				string messageBoxText = value.ToString();
				string caption = res.LanguageContent.Label_Error;
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;

				// TODO make assembly new dialog box to show brief error message and detailed information in assembly Expander
				// Display message box
				System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
			}
		}

		public string[] ErrorMessages {
			set {
				if (value != null) {
					string messageBoxText = string.Empty;
					for (int i = 0; i < value.Length; i++) {
						messageBoxText += res.LanguageContent.ResourceManager.GetString(value[i], 
							Thread.CurrentThread.CurrentCulture) + "\r\n";
					}
					string caption = HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.Label_Error;
					MessageBoxButton button = MessageBoxButton.OK;
					MessageBoxImage icon = MessageBoxImage.Error;

					// TODO make assembly new dialog box with same look and feel as above.
					// Display message box
					System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
				}
			}
		}

		public void ResetJobSize(int jobSize) {
			//this.Progress.Maximum = jobSize;
			//this.Progress.Value = 0;

			if (!this.Dispatcher.CheckAccess()) {
				this.Dispatcher.Invoke(
				  System.Windows.Threading.DispatcherPriority.Normal,
				  new Action(
					delegate() {
						this.Progress.Maximum = jobSize;
						this.Progress.Value = 0;
					}
				));
			} else {
				this.Progress.Maximum = jobSize;
				this.Progress.Value = 0;
			}
		}

		public void ReportProgress() {
			Progress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				new DispatcherOperationCallback(
					delegate {
						//System.Diagnostics.Debug.WriteLine("Progress bar value is: " + Progress.Value);
						Progress.Value = Progress.Value + 1;
						return null;
					}), null);
		}

		/*public void ProcessingStopped() {
			if (!this.Dispatcher.CheckAccess()) {
				this.Dispatcher.Invoke(
				  System.Windows.Threading.DispatcherPriority.Normal,
				  new Action(
					delegate() {
						this.processing = false;
					}
				));
			} else {
				this.processing = false;
			}
		}*/

		public void LoadWatermarkControl(int index) {
			if (ps.WatermarkCollection != null
				&& ps.WatermarkCollection.Count >= index + 1) {
				// scroll to right end to show the newly added one
				svWatermarkList.ScrollToRightEnd();
				// another solution is described at http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/b75cf780-051b-461c-ae14-9bd47cb32e0a/
				// to scroll to the new item even not at the end

				if (ps.WatermarkCollection[index] is HardySoft.UI.BatchImageProcessor.Model.WatermarkImage) {
					HardySoft.UI.BatchImageProcessor.Model.WatermarkImage watermarkImage = ps.WatermarkCollection[index] as HardySoft.UI.BatchImageProcessor.Model.WatermarkImage;
					WatermarkImage watermarkControl = (WatermarkImage)container.Resolve<WatermarkImage>();
					watermarkControl.WatermarkIndex = index;

					System.Windows.Data.Binding rotateAngleBinding = new System.Windows.Data.Binding();
					rotateAngleBinding.Source = watermarkImage;
					rotateAngleBinding.Path = new PropertyPath("WatermarkRotateAngle");
					rotateAngleBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkImage.WatermarkImageRotateAngleProperty, rotateAngleBinding);

					System.Windows.Data.Binding positionBinding = new System.Windows.Data.Binding();
					positionBinding.Source = watermarkImage;
					positionBinding.Path = new PropertyPath("WatermarkPosition");
					positionBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkImage.WatermarkImagePositionProperty, positionBinding);

					System.Windows.Data.Binding fileNameBinding = new System.Windows.Data.Binding();
					fileNameBinding.Source = watermarkImage;
					fileNameBinding.Path = new PropertyPath("WatermarkImageFile");
					fileNameBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkImage.WatermarkImageFileProperty, fileNameBinding);

					System.Windows.Data.Binding opacityBinding = new System.Windows.Data.Binding();
					opacityBinding.Source = watermarkImage;
					opacityBinding.Path = new PropertyPath("WatermarkImageOpacity");
					opacityBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkImage.WatermarkImageOpacityProperty, opacityBinding);

					System.Windows.Data.Binding paddingBinding = new System.Windows.Data.Binding();
					paddingBinding.Source = watermarkImage;
					paddingBinding.Path = new PropertyPath("Padding");
					paddingBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkImage.PaddingProperty, paddingBinding);

					watermarkControl.WatermarkImageFile = watermarkImage.WatermarkImageFile;
					watermarkControl.WatermarkImageRotateAngle = watermarkImage.WatermarkRotateAngle;

					WatermarkPlaceHolder.Content = watermarkControl;
				} else if (ps.WatermarkCollection[index] is HardySoft.UI.BatchImageProcessor.Model.WatermarkText) {
					HardySoft.UI.BatchImageProcessor.Model.WatermarkText watermarkText = ps.WatermarkCollection[index] as HardySoft.UI.BatchImageProcessor.Model.WatermarkText;
					WatermarkText watermarkControl = (WatermarkText)container.Resolve<WatermarkText>();
					watermarkControl.WatermarkIndex = index;

					System.Windows.Data.Binding rotateAngleBinding = new System.Windows.Data.Binding();
					rotateAngleBinding.Source = watermarkText;
					rotateAngleBinding.Path = new PropertyPath("WatermarkRotateAngle");
					rotateAngleBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextRotateAngleProperty, rotateAngleBinding);

					System.Windows.Data.Binding positionBinding = new System.Windows.Data.Binding();
					positionBinding.Source = watermarkText;
					positionBinding.Path = new PropertyPath("WatermarkPosition");
					positionBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextPositionProperty, positionBinding);

					System.Windows.Data.Binding textBinding = new System.Windows.Data.Binding();
					textBinding.Source = watermarkText;
					textBinding.Path = new PropertyPath("Text");
					textBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextToDisplayProperty, textBinding);

					System.Windows.Data.Binding fontBinding = new System.Windows.Data.Binding();
					fontBinding.Source = watermarkText;
					fontBinding.Path = new PropertyPath("WatermarkTextFont");
					fontBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextFontProperty, fontBinding);

					System.Windows.Data.Binding colorBinding = new System.Windows.Data.Binding();
					colorBinding.Source = watermarkText;
					colorBinding.Path = new PropertyPath("WatermarkTextColor");
					colorBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextColorProperty, colorBinding);

					System.Windows.Data.Binding aligmentBinding = new System.Windows.Data.Binding();
					aligmentBinding.Source = watermarkText;
					aligmentBinding.Path = new PropertyPath("WatermarkTextAlignment");
					aligmentBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.WatermarkTextAlignmentProperty, aligmentBinding);

					System.Windows.Data.Binding paddingBinding = new System.Windows.Data.Binding();
					paddingBinding.Source = watermarkText;
					paddingBinding.Path = new PropertyPath("Padding");
					paddingBinding.Mode = BindingMode.TwoWay;
					watermarkControl.SetBinding(WatermarkText.PaddingProperty, paddingBinding);

					WatermarkPlaceHolder.Content = watermarkControl;
				}
			}
		}

		public int? SelectedWatermarkIndex {
			get {
				if (lbWatermarkCollection.Items == null
					|| lbWatermarkCollection.Items.Count == 0) {
					return 0;
				} else {
					return lbWatermarkCollection.SelectedIndex;
				}
			}
			set {
				lbWatermarkCollection.SelectedIndex = value ?? value.Value;
			}
		}

		public void ClearWatermarkArea() {
			WatermarkPlaceHolder.Content = null;
		}

		/// <summary>
		/// Display warning message and collect user action.
		/// </summary>
		/// <param name="warningMessageResourceKey">Warning message resource key.</param>
		/// <returns>
		/// True if user clicks "Yes", otherwise False.
		/// </returns>
		public bool DisplayWarning(string warningMessageResourceKey) {
			string caption = HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.Label_Warning;
			/*string message = res.LanguageContent.ResourceManager.GetString(warningMessageResourceKey,
							Thread.CurrentThread.CurrentCulture);*/
			string message = Utilities.ParseResource(warningMessageResourceKey);
			MessageBoxButton button = MessageBoxButton.YesNo;
			MessageBoxImage icon = MessageBoxImage.Warning;
			if (System.Windows.MessageBox.Show(message, caption, button, icon) == MessageBoxResult.Yes) {
				return true;
			} else {
				return false;
			}
		}
		#endregion

		#region View events
		public event ProjectWithFileNameEventHandler NewProjectCreated;
		public event ProjectWithFileNameEventHandler SaveProject;
		public event ProjectWithFileNameEventHandler SaveProjectAs;
		public event ProjectWithFileNameEventHandler OpenProject;
		public event ProcessThreadNumberEventHandler ProcessImage;
		public event EventHandler StopProcessing;
		#endregion

		#region Commands
		#region New Command
		private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (this.presenter == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = !this.presenter.Processing;
			}
			/*if (this.presenter.Processing) {
				e.CanExecute = false;
			} else {
				e.CanExecute = true;
			}*/
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if (this.ps.IsDirty) {
				MessageBoxResult result = System.Windows.MessageBox.Show(res.LanguageContent.Message_SavePrompt,
					res.LanguageContent.Label_UnsavedProject,
					MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

				switch (result) {
					case MessageBoxResult.Yes:
						saveProject();
						newProject();
						break;
					case MessageBoxResult.No:
						newProject();
						break;
					default:
						// "Cancel" do nothing
						break;
				}
			} else {
				newProject();
			}
		}

		private void newProject() {
			ProjectWithFileNameEventHandler handlers = NewProjectCreated;
			if (handlers != null) {
				ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(res.LanguageContent.Label_UntitledProjectName);
				handlers(this, args);

				//this.currentProjectFile = "Untitled.hsbip";
				OnProjectFileNameObtained();
			}
		}
		#endregion

		#region Open Command
		private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (this.presenter == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = !this.presenter.Processing;
			}
			/*if (this.presenter.Processing) {
				e.CanExecute = false;
			} else {
				e.CanExecute = true;
			}*/
		}
		
		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if (this.ps.IsDirty) {
				MessageBoxResult result = System.Windows.MessageBox.Show(res.LanguageContent.Message_SavePrompt,
					res.LanguageContent.Label_UnsavedProject,
					MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

				switch (result) {
					case MessageBoxResult.Yes:
						saveProject();
						openProject();
						break;
					case MessageBoxResult.No:
						openProject();
						break;
					default:
						// "Cancel" do nothing
						break;
				}
			} else {
				openProject();
			}
		}

		private void openProject() {
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = res.LanguageContent.Label_AllProjects + " (*.hsbip)|*.hsbip;";
			openFile.Multiselect = false;
			if (openFile.ShowDialog() == DialogResult.OK) {
				string projectFileName = openFile.FileName;
				if (!string.IsNullOrEmpty(projectFileName)) {
					openProject(projectFileName);
				}
			}
		}

		private void openProject(string projectFileName) {
			ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(projectFileName);

			ProjectWithFileNameEventHandler handlers = OpenProject;
			if (handlers != null) {
				handlers(this, args);
			}

			//this.currentProjectFile = projectFileName;
			OnProjectFileNameObtained();
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
			saveProject();
		}

		private void saveProject() {
			if (File.Exists(this.presenter.CurrentProjectFile)) {
				// project file already created
				ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(this.presenter.CurrentProjectFile);

				ProjectWithFileNameEventHandler handlers = SaveProject;
				if (handlers != null) {
					handlers(this, args);
				}

				OnProjectFileNameObtained();
			} else {
				saveProjectAs();
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
			saveProjectAs();
		}

		private void saveProjectAs() {
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = res.LanguageContent.Label_AllProjects + " (*.hsbip)|*.hsbip;";
			if (saveFile.ShowDialog() == DialogResult.OK) {
				string projectFileName = saveFile.FileName;
				if (!string.IsNullOrEmpty(projectFileName)) {
					ProjectWithFileNameEventArgs args = new ProjectWithFileNameEventArgs(projectFileName);

					ProjectWithFileNameEventHandler handlers = SaveProjectAs;
					if (handlers != null) {
						handlers(this, args);
					}

					//this.currentProjectFile = projectFileName;
					OnProjectFileNameObtained();
				}
			}
		}
		#endregion

		#region Stop Command
		private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
#if DEBUG
			string message;
			if (this.presenter == null) {
				message = string.Format("Processing status is unknown at {0}.",
					DateTime.Now);
			} else {
				message = string.Format("Processing status is {0} at {1}.",
					this.presenter.Processing,
					DateTime.Now);
			}
			System.Diagnostics.Debug.WriteLine(message);
#endif
			if (this.presenter == null) {
				e.CanExecute = false;
			} else {
				e.CanExecute = this.presenter.Processing;
			}
			//e.CanExecute = this.presenter.Processing;
		}
		
		private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			EventHandler handlers = StopProcessing;
			if (handlers != null) {
				EventArgs args = new EventArgs();
				handlers(this, args);
			}
		}
		#endregion

		#region Make Command
		private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (ps == null) {
				e.CanExecute = false;
			} else {
				if (this.presenter.Processing) {
					e.CanExecute = false;
				} else {
					e.CanExecute = ps.ProjectCreated;
				}
			}
		}

		private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			ProcessThreadNumberEventHandler handlers = ProcessImage;
			if (handlers != null) {
				//this.processing = true;
				this.Progress.Value = 0;
				ProcessThreadNumberEventArgs args = new ProcessThreadNumberEventArgs(Properties.Settings.Default.ThreadNumber,
					Properties.Settings.Default.DateTimeFormatString);
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
			if (this.ps.IsDirty) {
				MessageBoxResult result = System.Windows.MessageBox.Show(res.LanguageContent.Message_SavePrompt,
					res.LanguageContent.Label_UnsavedProject,
					MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

				switch (result) {
					case MessageBoxResult.Yes:
						saveProject();
						System.Windows.Application.Current.Shutdown();
						break;
					case MessageBoxResult.No:
						System.Windows.Application.Current.Shutdown();
						break;
					default:
						// "Cancel" do nothing
						break;
				}
			} else {
				System.Windows.Application.Current.Shutdown();
			}
		}
		#endregion

		#region Preference window command
		private void PreferenceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void PreferenceCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			Preference preferenceWindow = (Preference)container.Resolve<Preference>();
			preferenceWindow.Owner = Window.GetWindow(this);
			preferenceWindow.ShowDialog();
		}
		#endregion

		#region About window command
		private void AboutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			About about = new About();
			about.Owner = Window.GetWindow(this);
			about.ShowDialog();
		}
		#endregion

		#region Help context button command
		private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			HelpPopup popup = new HelpPopup();

			// Mouse position
			System.Windows.Point mousePoint = this.PointToScreen(Mouse.GetPosition(this));
			//System.Windows.Point mousePoint = Mouse.GetPosition(this);
			popup.Owner = Window.GetWindow(this);
			popup.ShowDialog(mousePoint.X, mousePoint.Y, (string)e.Parameter);
		}

		private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}
		#endregion

		#region Discuss forum command
		private void DiscussCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			System.Diagnostics.Process.Start("http://groups.google.com/group/sea-turtle-batch-image-processor");
		} 
		#endregion

		#region Add Watermark image/text
		private void AddWatermark_Executed(object sender, ExecutedRoutedEventArgs e) {
			switch (e.Parameter as string) {
				case "WatermarkImage":
					presenter.AddWatermarkImage();
					break;
				case "WatermarkText":
					presenter.AddWatermarkText();
					break;
			}
		}
		#endregion

		#region Delete watermark image/text
		private void DeleteWatermark_Executed(object sender, ExecutedRoutedEventArgs e) {
			int index = Convert.ToInt32(e.Parameter);

			presenter.RemoveWatermark(index);
		} 
		#endregion
		#endregion
	}
}