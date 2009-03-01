using System;
using System.Threading;
using System.Windows;

using HardySoft.UI.BatchImageProcessor.Classes;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		IUnityContainer container = new UnityContainer();

		private void Application_Startup(object sender, StartupEventArgs e) {
#if DEBUG
			//Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
#endif
			MainWindow mainWindow = (MainWindow)container.Resolve<MainWindow>();
			mainWindow.Show();

			// TODO create a resource library to separate UI resource from main project, P345
			// TODO add skin feature, P472

			Thread t = new Thread(new ThreadStart(checkVersion));
			t.Start();
		}

		private void checkVersion() {
			/*this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				(ThreadStart)delegate() {
				}
			);*/

			Version latestVersion = AssembyInformation.GetLatestVersion();
			Version myVersion = AssembyInformation.GetApplicationVersion();
			if (latestVersion != null && myVersion != null) {
				this.Dispatcher.BeginInvoke(
					  new Action(
						delegate() {
							compareVersion(latestVersion, myVersion);
						}
					)
				);
			}
		}

		private void compareVersion(Version latestVersion, Version myVersion) {
			if (latestVersion > myVersion) {
				showNewVersionWindow(latestVersion, myVersion);
			}

#if DEBUG
			//showNewVersionWindow(latestVersion, myVersion);
#endif
		}

		private void showNewVersionWindow(Version latestVersion, Version myVersion) {
			string compareStatus = string.Format(HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.Message_NewVersionAvailable,
				latestVersion.ToString());

			VersionCheckingResult window = new VersionCheckingResult();
			window.LatestVersion = latestVersion;
			window.MyVersion = myVersion;
			window.VersionCompareStatus = compareStatus;
			window.ApplicationURL = HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.ApplicationUrl;
			window.Show();
		}
	}
}