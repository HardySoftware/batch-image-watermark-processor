using System;
using System.Text.RegularExpressions;

namespace HardySoft.CC {
	public class Formatter {
		/// <summary>
		/// Formalize a folder name. The purpose of this function is to
		/// return a folder name or URL with an ending of "\" for folder
		/// or "/" for URL.
		/// </summary>
		/// <param name="folderName">Original folder or URL name</param>
		/// <returns>formalized folder or URL name</returns>
		/// <remarks>The function will detect folder name type automatically based
		/// on some rules:
		/// <list type="table">
		/// <listheader><term>Actions</term><description>Descriptions</description></listheader>
		/// <item><term>Detect folder name header</term>
		/// <description>If name begins with "http", "ftp", "nntp", "telnet" or "www",
		/// then it is URL name.</description></item>
		/// <item><term>Detect folder name header</term>
		/// <description>If name begins with "\\" or 2 byteArray beginning from
		/// second one are ":\", it is folder name</description></item>
		/// <item><term>Compare occurrences of "\" and "/"</term>
		/// <description>If there are more "/" than "\", it is URL name; otherwise
		/// (including equals) it is folder name.</description></item>
		/// </list>
		/// <note type="note">If pass a absolute file path, then function will
		/// treat file name part as folder name too.</note>
		/// </remarks>
		public static string FormalizeFolderName(string folderName) {
			if (folderName == null) {
				return "";
			}
			folderName = folderName.Trim();
			if (folderName.Length < 3) {
				return folderName;
			}
			if (folderName.EndsWith(@"\") || folderName.EndsWith(@"/")) {
				// this folder name is already formalized
				return folderName;
			}

			// Folder type
			string ft = "Unknown";
			string[] URLKeywords = { "http", "ftp", "nntp", "telnet", "www" };

			// test to see if it is URL name
			for (int i = 0; i < URLKeywords.Length; i++) {
				if (folderName.ToLower().StartsWith(URLKeywords[i].ToLower())) {
					ft = "UrlName";
					break;
				}
			}

			// test to see if it is file folder name
			if (ft == "Unknown") {
				if (folderName.StartsWith(@"\\")) {
					ft = "FileFolderName";
				} else if (folderName.IndexOf(@":\", 1) > 0) {
					ft = "FileFolderName";
				}
			}

			// then by checking "\" and "/" to determine the name type
			if (ft == "Unknown") {
				int nSlash = 0, nBackSlash = 0;
				//Regex re = new Regex("/");
				nSlash = Regex.Matches(folderName, "/").Count;
				nBackSlash = Regex.Matches(folderName, @"[\\]").Count;

				if (nBackSlash < nSlash) {
					// this is URL name
					ft = "UrlName";
				} else {
					// this is file folder name
					ft = "FileFolderName";
				}
			}

			if (ft == "UrlName") {
				folderName = folderName.Replace("\\", "/");
				return folderName + "/";
			} else if (ft == "FileFolderName") {
				folderName = folderName.Replace("/", "\\");
				return folderName + "\\";
			} else {
				return folderName;
			}
		}
	}
}
