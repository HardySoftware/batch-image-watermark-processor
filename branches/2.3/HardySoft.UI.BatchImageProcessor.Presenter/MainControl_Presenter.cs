using System;
using System.Collections.Generic;
/*using System.Linq;
using System.Text;*/
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

using Microsoft.Practices.EnterpriseLibrary.Validation;

using HardySoft.UI.BatchImageProcessor.View;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class MainControl_Presenter {
		private IMainInterfaceControlView view;
		private ProjectSetting ps;
		private ImageProcessorEngine engine = null;
		private bool processing;
		private string currentProjectFile;

		public MainControl_Presenter() {
			ps = new ProjectSetting();

			this.processing = false;
		}

		public void SetView(IMainInterfaceControlView view) {
			this.view = view;
			this.view.NewProjectCreated += new ProjectWithFileNameEventHandler(view_NewProjectCreated);
			this.view.SaveProject += new ProjectWithFileNameEventHandler(view_SaveProject);
			this.view.SaveProjectAs += new ProjectWithFileNameEventHandler(view_SaveProjectAs);
			this.view.OpenProject += new ProjectWithFileNameEventHandler(view_OpenProject);
			this.view.ProcessImage += new ProcessThreadNumberEventHandler(view_ProcessImage);
			this.view.StopProcessing += new EventHandler(view_StopProcessing);

			view.PS = ps;
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
			this.view.ErrorMessage = ex;
		}

		public void SetErrorMessage(string[] messages) {
			this.view.ErrorMessages = messages;
		}

		void view_OpenProject(object sender, ProjectWithFileNameEventArgs e) {
			Stream stream = new FileStream(e.ProjectFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try {
				IFormatter formatter = new BinaryFormatter();

				ps = (ProjectSetting)formatter.Deserialize(stream);

				// wire events again
				ps.OpenProject();
				view.PS = ps;

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
			view.PS = ps;

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

			engine = new ImageProcessorEngine(this.ps, threadNumber, events);

			view.ResetJobSize(engine.JobSize);

			engine.ImageProcessed += new ImageProcessedDelegate(engine_ImageProcessed);
			engine.StartProcess();

			AutoResetEvent.WaitAll(events);

			//this.view.ProcessingStopped();
			this.processing = false;
		}

		void engine_ImageProcessed(ImageProcessedEventArgs args) {
			view.ReportProgress();
		}

		void view_StopProcessing(object sender, EventArgs e) {
			if (engine != null) {
				engine.StopProcess();
			}
		}
	}
}