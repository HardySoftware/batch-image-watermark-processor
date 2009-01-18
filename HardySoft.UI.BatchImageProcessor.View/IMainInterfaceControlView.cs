using System;
using System.ComponentModel;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.View {
	public delegate void ProjectWithFileNameEventHandler(object sender, ProjectWithFileNameEventArgs e);
	public delegate void ProcessThreadNumberEventHandler(object sender, ProcessThreadNumberEventArgs e);

	public interface IMainInterfaceControlView {
		ProjectSetting PS {
			get;
			set;
		}

		Exception ErrorMessage {
			//get;
			set;
		}

		event EventHandler NewProjectCreated;
		event ProjectWithFileNameEventHandler SaveProject;
		event ProjectWithFileNameEventHandler SaveProjectAs;
		event ProjectWithFileNameEventHandler OpenProject;
		event ProcessThreadNumberEventHandler ProcessImage;
		event EventHandler StopProcessing;
	}
}
