using System;

namespace HardySoft.UI.BatchImageProcessor.View {
	public class ProjectWithFileNameEventArgs : EventArgs {
		private string projectFileName;

		public ProjectWithFileNameEventArgs(string projectFileName)
			: base() {
			this.projectFileName = projectFileName;
		}

		public string ProjectFileName {
			get {
				return this.projectFileName;
			}
		}
	}
}
