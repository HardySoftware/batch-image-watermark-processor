using System;
/*using System.Collections.Generic;
using System.Linq;
using System.Text;*/
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using HardySoft.UI.BatchImageProcessor.View;
using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public class MainControl_Presenter {
		private IMainInterfaceControlView view;
		private ProjectSetting ps;

		public MainControl_Presenter() {
			ps = new ProjectSetting();
		}

		public void SetView(IMainInterfaceControlView view) {
			this.view = view;
			this.view.NewProjectCreated += new EventHandler(view_NewProjectCreated);
			this.view.SaveProject += new ProjectWithFileNameEventHandler(view_SaveProject);
			this.view.SaveProjectAs += new ProjectWithFileNameEventHandler(view_SaveProjectAs);
			this.view.OpenProject += new ProjectWithFileNameEventHandler(view_OpenProject);

			view.PS = ps;
		}

		public void SetErrorMessage(Exception ex) {
			this.view.ErrorMessage = ex;
		}

		void view_OpenProject(object sender, ProjectWithFileNameEventArgs e) {
			Stream stream = new FileStream(e.ProjectFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try {
				IFormatter formatter = new BinaryFormatter();

				ps = (ProjectSetting)formatter.Deserialize(stream);

				ps.OpenProject();
				view.PS = ps;
			} catch (Exception ex) {
				SetErrorMessage(ex);
			}
			finally {
				stream.Close();
			}
		}

		private bool saveProject(string projectFileName) {
			Stream stream = new FileStream(projectFileName, FileMode.Create, FileAccess.Write, FileShare.None);
			try {
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, ps);

				// reset is dirty flag
				ps.SaveProject();
				return true;
			} catch (Exception ex) {
				SetErrorMessage(ex);
				return false;
			}
			finally {
				stream.Close();
			}
		}

		void view_SaveProjectAs(object sender, ProjectWithFileNameEventArgs e) {
			saveProject(e.ProjectFileName);
		}

		void view_SaveProject(object sender, ProjectWithFileNameEventArgs e) {
			saveProject(e.ProjectFileName);
		}

		void view_NewProjectCreated(object sender, EventArgs e) {
			ps = new ProjectSetting();
			ps.NewProject();
			view.PS = ps;
		}
	}
}
