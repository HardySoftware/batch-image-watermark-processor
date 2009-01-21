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
		public string GetFileName(string sourceFile, ProjectSetting ps) {
			string prefix = ps.ThumbnailSetting.ThumbnailFileNamePrefix;
			string suffix = ps.ThumbnailSetting.ThumbnailFileNameSuffix;

			if (string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(suffix)) {
				// default thumbnail file name part
				suffix = "_thumb";
			}

			string fileName, fileNameWithoutExtension, extension, thumbFileName;
			FileInfo fi = new FileInfo(sourceFile);
			fileName = fi.Name;
			fileNameWithoutExtension = fileName.Remove(fileName.IndexOf(fi.Extension));
			extension = fi.Extension;

			thumbFileName = prefix + fileNameWithoutExtension + suffix + extension;
			thumbFileName = Formatter.FormalizeFolderName(ps.OutputDirectory) + thumbFileName;

			return thumbFileName;
		}

		private uint imageIndex;
		public uint ImageIndex {
			get { return imageIndex; }
			set { imageIndex = value; }
		}
	}
}
