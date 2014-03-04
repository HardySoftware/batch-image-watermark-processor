using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using HardySoft.CC;

using HardySoft.UI.BatchImageProcessor.Classes;
using HardySoft.UI.BatchImageProcessor.View;

using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private IUnityContainer container = new UnityContainer();
		private IConfiguration extraConfig;
		private MainWindow mainWindow;

		public App() {
			this.container.RegisterType<IConfiguration, ExtraConfiguration>();
			// depedency injection
			this.extraConfig = container.Resolve<ExtraConfiguration>();

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
		}

		private void Application_Startup(object sender, StartupEventArgs e) {
			#region Command line arguments
			CommandArgument commands = new CommandArgument(e.Args);
			// additional command line arguments to control some behaviors of application
			// For example
			//      SeaTurtle.exe /L:en-CA /Debug:true
			if (commands["L"] != null) {
				// force language selection
				try {
					Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(commands["L"]);
					extraConfig.ApplicationLanguage = Thread.CurrentThread.CurrentCulture;
				} catch (ArgumentException) {
					string text = string.Format("{0} is not a valid culture code.",
						commands["L"]);
					System.Windows.MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}

#if DEBUG
#else
			if (commands["debug"] != null) {
				if (string.Compare(commands["debug"], "true", true) == 0) {
#endif
					string logDir = Formatter.FormalizeFolderName(Environment.CurrentDirectory) + @"Log\";
					if (!Directory.Exists(logDir)) {
						Directory.CreateDirectory(logDir);
					}

					TextWriterTraceListener listener = new TextWriterTraceListener(logDir + "application.log");
					Trace.Listeners.RemoveAt(0);
					Trace.Listeners.Add(listener);
					Trace.AutoFlush = true;
					Trace.UseGlobalLock = true;
#if DEBUG
#else
				}
			}
#endif
			#endregion

			mainWindow = (MainWindow)container.Resolve<MainWindow>();
			mainWindow.SetSkin();
			mainWindow.Show();

			// TODO create assembly resource library to separate UI resource from main project, P345
			
			// TODO make button image to support skin

			Thread t = new Thread(new ThreadStart(checkVersion));
			t.Start();
		}

		private void checkVersion() {
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

		#region Unhandled exceptions
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
			Trace.TraceError(e.Exception.ToString());
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Exception ex = (Exception)e.ExceptionObject;

			Trace.TraceError(ex.Message);
			Console.WriteLine(ex.Message);
		}
		#endregion

		private void Application_Exit(object sender, ExitEventArgs e) {
			return;
		}
	}
}