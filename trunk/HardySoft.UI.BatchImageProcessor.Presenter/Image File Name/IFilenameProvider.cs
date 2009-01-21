using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface IFilenameProvider {
		string GetFileName(string sourceFile, ProjectSetting ps);
		uint ImageIndex {
			get;
			set;
		}
	}
}
