using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.CC;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to generate thumbnail image file name.
	/// </summary>
	class ThumbnailFileName : IFilenameProvider {
		public string GetFileName() {
			string prefix = PS.ThumbnailSetting.ThumbnailFileNamePrefix;
			string suffix = PS.ThumbnailSetting.ThumbnailFileNameSuffix;

			if (string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(suffix)) {
				// default thumbnail file name part
				suffix = "_thumb";
			}

			string fileName, fileNameWithoutExtension, extension, thumbFileName;
			FileInfo fi = new FileInfo(SourceFileName);
			fileName = fi.Name;
			fileNameWithoutExtension = fileName.Remove(fileName.IndexOf(fi.Extension));
			extension = fi.Extension;

			thumbFileName = prefix + fileNameWithoutExtension + suffix + extension;
			thumbFileName = Formatter.FormalizeFolderName(PS.OutputDirectory) + thumbFileName;

			return thumbFileName;
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
