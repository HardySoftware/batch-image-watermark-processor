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

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			// Set an icon using code
			Uri iconUri = new Uri("pack://application:,,,/Images/icon.png", UriKind.RelativeOrAbsolute);
			this.Icon = BitmapFrame.Create(iconUri);

			IUnityContainer container = new UnityContainer();
			MainInterfaceControl mainControl = (MainInterfaceControl)container.Resolve<MainInterfaceControl>();
			MainControlPlaceHolder.Children.Add(mainControl);
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
	}
}
