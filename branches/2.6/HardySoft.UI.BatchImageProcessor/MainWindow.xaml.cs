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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;

using HardySoft.UI.BatchImageProcessor.Controls;
using HardySoft.UI.BatchImageProcessor.Classes;
using HardySoft.UI.BatchImageProcessor.Classes.Commands;
using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		MainInterfaceControl mainControl;

		public MainWindow() {
			InitializeComponent();

			IUnityContainer container = new UnityContainer();
			mainControl = (MainInterfaceControl)container.Resolve<MainInterfaceControl>();
			mainControl.ProjectFileNameObtained += new ProjectFileNameObtainedHandler(mainControl_ProjectFileNameObtained);
			MainControlPlaceHolder.Children.Add(mainControl);

			// command / focus issue
			// see http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/753c2a0b-753f-43d3-afb3-01d4d3c93787
			KeyGesture keyO = new KeyGesture(Key.O, ModifierKeys.Control);
			KeyBinding kbO = new KeyBinding(ApplicationCommands.Open, keyO);
			kbO.CommandTarget = mainControl;
			this.InputBindings.Add(kbO);

			KeyGesture keyN = new KeyGesture(Key.N, ModifierKeys.Control);
			KeyBinding kbN = new KeyBinding(ApplicationCommands.New, keyN);
			kbN.CommandTarget = mainControl;
			this.InputBindings.Add(kbN);

			KeyGesture keyS = new KeyGesture(Key.S, ModifierKeys.Control);
			KeyBinding kbS = new KeyBinding(ApplicationCommands.Save, keyS);
			kbS.CommandTarget = mainControl;
			this.InputBindings.Add(kbS);

			KeyGesture keySA = new KeyGesture(Key.H, ModifierKeys.Control);
			KeyBinding kbSA = new KeyBinding(ApplicationCommands.SaveAs, keySA);
			kbSA.CommandTarget = mainControl;
			this.InputBindings.Add(kbSA);

			KeyGesture keyESC = new KeyGesture(Key.Escape, ModifierKeys.Control);
			KeyBinding kbESC = new KeyBinding(ApplicationCommand.Exit, keyESC);
			kbESC.CommandTarget = mainControl;
			this.InputBindings.Add(kbESC);

			KeyGesture keyM = new KeyGesture(Key.M, ModifierKeys.Control);
			KeyBinding kbM = new KeyBinding(ApplicationCommand.Make, keyM);
			kbM.CommandTarget = mainControl;
			this.InputBindings.Add(kbM);
		}

		void mainControl_ProjectFileNameObtained(object sender, ProjectFileNameEventArgs args) {
			string fileName = "";
			if (args.IsDirty) {
				fileName = "* ";
			}
			fileName += args.ProjectFileName;
			tbFooter.Text = fileName;
		}

		bool isWiden = false;
		private void window_initiateWiden(object sender, System.Windows.Input.MouseEventArgs e) {
			isWiden = true;
		}

		private void window_endWiden(object sender, System.Windows.Input.MouseEventArgs e) {
			isWiden = false;

			// Make sure capture is released.
			Rectangle rect = (Rectangle)sender;
			rect.ReleaseMouseCapture();
		}

		private void window_Widen(object sender, System.Windows.Input.MouseEventArgs e) {
			Rectangle rect = (Rectangle)sender;
			if (isWiden) {
				rect.CaptureMouse();
				double newWidth = e.GetPosition(this).X + 5;
				if (newWidth > 0)
					this.Width = newWidth;
			}
		}

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			this.DragMove();
		}

		private IConfiguration hiddenConfig;
		[Dependency]
		public IConfiguration HiddenConfig {
			get {
				return this.hiddenConfig;
			}
			set {
				this.hiddenConfig = value;
				mainControl.HiddenConfig = value;
			}
		}
	}
}