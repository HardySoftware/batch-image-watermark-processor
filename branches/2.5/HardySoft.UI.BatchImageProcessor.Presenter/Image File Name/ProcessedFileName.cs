using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.CC;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to generate regular output file name.
	/// </summary>
	class ProcessedFileName : IFilenameProvider {
		public string GetFileName() {
			FileInfo fi = new FileInfo(SourceFileName);
			return Formatter.FormalizeFolderName(PS.OutputDirectory) + fi.Name;
		}

		public uint? ImageIndex {
			get;
			set;
		}

		public ProjectSetting PS {
			get;
			set;
		}

		public string SourceFileName {
			get;
			set;
		}
	}
}
