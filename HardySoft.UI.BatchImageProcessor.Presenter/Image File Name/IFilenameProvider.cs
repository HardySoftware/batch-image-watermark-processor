using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HardySoft.UI.BatchImageProcessor.Model;
using Microsoft.Practices.Unity;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	interface IFilenameProvider {
		string GetFileName();

		ProjectSetting PS {
			get;
			set;
		}

		uint? ImageIndex {
			get;
			set;
		}

		string SourceFileName {
			get;
			set;
		}
	}
}
