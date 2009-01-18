using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void Application_Startup(object sender, StartupEventArgs e) {
			IUnityContainer container = new UnityContainer();

			MainWindow mainWindow = (MainWindow)container.Resolve<MainWindow>();
			mainWindow.Show();
		}
	}
}
