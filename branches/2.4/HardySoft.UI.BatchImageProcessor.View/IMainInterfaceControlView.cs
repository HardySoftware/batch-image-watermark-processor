using System;
using System.ComponentModel;
using System.Collections.Generic;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.View {
	public delegate void ProjectWithFileNameEventHandler(object sender, ProjectWithFileNameEventArgs e);
	public delegate void ProcessThreadNumberEventHandler(object sender, ProcessThreadNumberEventArgs e);

	public interface IMainInterfaceControlView : IView {
		ProjectSetting PS {
			get;
			set;
		}

		Exception ErrorMessage {
			//get;
			set;
		}

		string[] ErrorMessages {
			set;
		}

		void ResetJobSize(int jobSize);
		void ReportProgress();
		//void ProcessingStopped();

		event ProjectWithFileNameEventHandler NewProjectCreated;
		event ProjectWithFileNameEventHandler SaveProject;
		event ProjectWithFileNameEventHandler SaveProjectAs;
		event ProjectWithFileNameEventHandler OpenProject;
		event ProcessThreadNumberEventHandler ProcessImage;
		event EventHandler StopProcessing;

		ExtraConfiguration HiddenConfig {
			get;
			set;
		}
	}
}
