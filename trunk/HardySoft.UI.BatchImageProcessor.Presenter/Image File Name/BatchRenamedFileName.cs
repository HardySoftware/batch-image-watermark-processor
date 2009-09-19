using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.CC;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to generate regaular output file with batch rename enabled.
	/// </summary>
	class BatchRenamedFileName : IFilenameProvider {
		public string GetFileName() {
			string fileNameWithoutExtension, extension, newFileName;
			FileInfo fi = new FileInfo(SourceFileName);
			string fileName = fi.Name;
			fileNameWithoutExtension = fileName.Remove(fileName.IndexOf(fi.Extension));
			extension = fi.Extension;

			if (! string.IsNullOrEmpty(PS.RenamingSetting.OutputFileNamePrefix)
				|| ! string.IsNullOrEmpty(PS.RenamingSetting.OutputFileNameSuffix)) {
				newFileName = PS.RenamingSetting.OutputFileNamePrefix
					+ ((ImageIndex.HasValue ? ImageIndex.Value : 0) + PS.RenamingSetting.StartNumber).ToString("D" + PS.RenamingSetting.NumberPadding) 
					+ PS.RenamingSetting.OutputFileNameSuffix;
			} else {
				newFileName = fileNameWithoutExtension + "_"
					+ ((ImageIndex.HasValue ? ImageIndex.Value : 0) + PS.RenamingSetting.StartNumber).ToString("D" + PS.RenamingSetting.NumberPadding);
			}
			newFileName = newFileName + extension;
			newFileName = Formatter.FormalizeFolderName(PS.OutputDirectory) + newFileName;

			switch (PS.RenamingSetting.FileNameCase) {
				case OutputFileNameCase.LowerCase:
					newFileName = newFileName.ToLower();
					break;
				case OutputFileNameCase.UpperCase:
					newFileName = newFileName.ToUpper();
					break;
			}
			return newFileName;
		}

		public ProjectSetting PS {
			get;
			set;
		}

		public uint? ImageIndex {
			get;
			set;
		}

		public string SourceFileName {
			get;
			set;
		}
	}
}
