using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HardySoft.BatchImageProcessor.ControlFileCreator {
	class Program {
		static void Main(string[] args) {
			if (args == null || args.Length < 2) {
				return;
			}

			string folder = args[0];
			string initialTag = args[1].Trim();
			string tagValue = string.Empty;

			if (args.Length > 2) {
				// the 3rd parameter is value of the tag.
				tagValue = args[2].Trim();
			}

			string[] files = Directory.GetFiles(folder, "*.jpg");

			foreach (string imageFile in files) {
				FileInfo fi = new FileInfo(imageFile);

				string imageFileNameOnly = fi.Name.Remove(fi.Name.Length - fi.Extension.Length);

				string controlFile = imageFileNameOnly + ".txt";

				string tagValueToUse;
				if (string.IsNullOrEmpty(tagValue)) {
					tagValueToUse = imageFileNameOnly;
				} else {
					tagValueToUse = tagValue;
				}

				File.WriteAllText(Path.Combine(folder, controlFile), "{{" + initialTag + "}} " + tagValueToUse, Encoding.UTF8);
			}
		}
	}
}
