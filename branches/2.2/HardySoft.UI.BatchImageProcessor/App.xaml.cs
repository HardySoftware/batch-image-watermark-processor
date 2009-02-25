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
			MainWindow mainWindow = (MainWindow)container.Resolve<MainWindow>();
			mainWindow.Show();

			// TODO create a resource library to separate UI resource from main project, P345
			// TODO add skin feature, P472

			Thread t = new Thread(new ThreadStart(checkVersion));
			t.Start();
		}

		private void checkVersion() {
			this.Dispatcher.BeginInvoke(
				  new Action(
					delegate() {
						Version latestVersion = AssembyInformation.GetLatestVersion();
						Version myVersion = AssembyInformation.GetApplicationVersion();

						if (latestVersion != null && myVersion != null) {
							compareVersion(latestVersion, myVersion);
						}
					}
				)
			);
		}

		private void compareVersion(Version latestVersion, Version myVersion) {
			if (latestVersion.Major > myVersion.Major) {
				showNewVersionWindow(latestVersion, myVersion);
			} else if (latestVersion.Minor > myVersion.Minor) {
				showNewVersionWindow(latestVersion, myVersion);
			} else if (latestVersion.Build > myVersion.Build) {
				showNewVersionWindow(latestVersion, myVersion);
			} else if (latestVersion.Revision > myVersion.Revision) {
				showNewVersionWindow(latestVersion, myVersion);
			}

#if DEBUG
			showNewVersionWindow(latestVersion, myVersion);
#endif
		}

		private void showNewVersionWindow(Version latestVersion, Version myVersion) {
			string compareStatus = string.Format(HardySoft.UI.BatchImageProcessor.Resources.LanguageContent.NewVersionAvailable,
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