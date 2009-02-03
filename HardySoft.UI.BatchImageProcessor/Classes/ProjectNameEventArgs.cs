using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Classes {
	public class ProjectFileNameEventArgs : EventArgs {
		private string projectFileName;
		public string ProjectFileName {
			get {
				return projectFileName;
			}
		}

		private bool isDirty;
		public bool IsDirty {
			get {
				return isDirty;
			}
		}

		public ProjectFileNameEventArgs(string projectFileName, bool isDirty)
			: base() {
			this.projectFileName = projectFileName;
			this.isDirty = isDirty;
		}
	}

	public delegate void ProjectFileNameObtainedHandler(object sender, ProjectFileNameEventArgs args);
}
