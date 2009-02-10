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
		public string GetFileName(string sourceFile, ProjectSetting ps) {
			FileInfo fi = new FileInfo(sourceFile);
			return Formatter.FormalizeFolderName(ps.OutputDirectory) + fi.Name;
		}

		private uint imageIndex;
		public uint ImageIndex {
			get { return imageIndex; }
			set { imageIndex = value; }
		}
	}
}
