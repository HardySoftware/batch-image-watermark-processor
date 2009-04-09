using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.UI.BatchImageProcessor.View;

using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class MainControl_Presenter : Presenter<IMainInterfaceControlView> {
		private ProjectSetting ps;
		private ImageProcessorEngine engine = null;
		private bool processing;
		private string currentProjectFile;

		public MainControl_Presenter() {
			ps = new ProjectSetting();

			this.processing = false;
		}

		public override void SetView(IMainInterfaceControlView view) {
			this.View = view;
			this.View.NewProjectCreated += new ProjectWithFileNameEventHandler(view_NewProjectCreated);
			this.View.SaveProject += new ProjectWithFileNameEventHandler(view_SaveProject);
			this.View.SaveProjectAs += new ProjectWithFileNameEventHandler(view_SaveProjectAs);
			this.View.OpenProject += new ProjectWithFileNameEventHandler(view_OpenProject);
			this.View.ProcessImage += new ProcessThreadNumberEventHandler(view_ProcessImage);
			this.View.StopProcessing += new EventHandler(view_StopProcessing);

			this.View.ExifTag = getExifTags();

			this.View.PS = ps;
		}

		public bool Processing {
			get {
				return this.processing;
			}
		}

		public string CurrentProjectFile {
			get {
				return this.currentProjectFile;
			}
		}

		public void SetErrorMessage(Exception ex) {
			this.View.ErrorMessage = ex;
		}

		public void SetErrorMessage(string[] messages) {
			this.View.ErrorMessages = messages;
		}

		private Dictionary<string, string> getExifTags() {
			Dictionary<string, string> tags = new Dictionary<string, string>();
			PropertyInfo[] pi = typeof(ExifMetadata).GetProperties();
			for (int i = 0; i < pi.Length; i++) {
				object[] attr = pi[i].GetCustomAttributes(true);

				for (int j = 0; j < attr.Length; j++) {
					if (attr[j] is ExifDisplayAttribute) {
						ExifDisplayAttribute exifAttri = (ExifDisplayAttribute)attr[j];

						string displayName = exifAttri.DisplayName;
						string propertyName = pi[i].Name;
						tags.Add(propertyName, displayName);
					}
				}
			}

			return tags;
		}

		void view_OpenProject(object sender, ProjectWithFileNameEventArgs e) {
			Stream stream = new FileStream(e.ProjectFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try {
				IFormatter formatter = new BinaryFormatter();

				ps = (ProjectSetting)formatter.Deserialize(stream);

				// wire events again
				ps.OpenProject();
				View.PS = ps;

				this.currentProjectFile = e.ProjectFileName;
			} catch (Exception ex) {
				SetErrorMessage(ex);
			} finally {
				stream.Close();
				this.processing = false;
			}
		}

		private bool saveProject(string projectFileName) {
			Stream stream = new FileStream(projectFileName, FileMode.Create, FileAccess.Write, FileShare.None);
			try {
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, ps);

				// reset is dirty flag
				ps.SaveProject();

				this.currentProjectFile = projectFileName;

				return true;
			} catch (Exception ex) {
				SetErrorMessage(ex);
				return false;
			} finally {
				stream.Close();
			}
		}

		void view_SaveProjectAs(object sender, ProjectWithFileNameEventArgs e) {
			saveProject(e.ProjectFileName);
		}

		void view_SaveProject(object sender, ProjectWithFileNameEventArgs e) {
			saveProject(e.ProjectFileName);
		}

		void view_NewProjectCreated(object sender, ProjectWithFileNameEventArgs e) {
			ps = new ProjectSetting();
			ps.NewProject();
			View.PS = ps;

			//this.currentProjectFile = "Untitled.hsbip";
			this.currentProjectFile = e.ProjectFileName;
			this.processing = false;
		}

		void view_ProcessImage(object sender, ProcessThreadNumberEventArgs e) {
			ValidationResults results = Validation.Validate<ProjectSetting>(ps);
			if (!results.IsValid) {
				List<string> exceptions = new List<string>();
				foreach (ValidationResult vr in results) {
					exceptions.Add(vr.Message);
				}

				SetErrorMessage(exceptions.ToArray());
			} else {
				this.processing = true;

				// we need to use WaitAll to be notified all jobs from all threads are done,
				// WaitAll will block the current thread, I don't want it happen to main thread,
				// that is the reason we create another thread instead.
				Thread controlThread = new Thread(new ParameterizedThreadStart(engineController));
				controlThread.Start(e.ThreadNumber);
			}
		}

		private void engineController(object state) {
			uint threadNumber = (uint)state;
			AutoResetEvent[] events = new AutoResetEvent[threadNumber];
			for (int i = 0; i < events.Length; i++) {
				events[i] = new AutoResetEvent(false);
			}

			engine = new ImageProcessorEngine(this.ps, threadNumber, events,
				View.HiddenConfig.EnableDebug);

			View.ResetJobSize(engine.JobSize);

			engine.ImageProcessed += new ImageProcessedDelegate(engine_ImageProcessed);
			engine.StartProcess();

			AutoResetEvent.WaitAll(events);

			//this.view.ProcessingStopped();
			this.processing = false;
		}

		void engine_ImageProcessed(ImageProcessedEventArgs args) {
			View.ReportProgress();
		}

		void view_StopProcessing(object sender, EventArgs e) {
			if (engine != null) {
				engine.StopProcess();
			}
		}
	}
}