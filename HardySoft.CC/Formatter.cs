using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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

		/// <summary>
		/// Serialize an object into in memory Xml string.
		/// </summary>
		/// <typeparam name="T">Object type to serialize.</typeparam>
		/// <param name="obj">Object instance to serialize.</param>
		/// <returns>Xml string to represent object.</returns>
		public static string Serializer<T>(T obj) {
			MemoryStream ms = new MemoryStream();
			XmlSerializer xs = new XmlSerializer(typeof(T));
			xs.Serialize(ms, obj);

			ms.Seek(0, 0);

			// Create a stream reader.
			using (StreamReader reader = new StreamReader(ms)) {
				// Just read to the end.
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// De-serialize a valid in momory Xml string into object.
		/// </summary>
		/// <typeparam name="T">Object type to de-serialize.</typeparam>
		/// <param name="xml">In momory Xml string.</param>
		/// <returns>Object instance represented by Xml string.</returns>
		public static T DeSerializer<T>(string xml) {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			object obj = xs.Deserialize(new StringReader(xml));

			return (T)obj;
			// return default(T);
		}

		/// <summary>
		/// De-serialize a valid in momory Xml stream string into object.
		/// </summary>
		/// <typeparam name="T">Object type to de-serialize.</typeparam>
		/// <param name="xml">In momory Xml stream.</param>
		/// <returns>Object instance represented by Xml string.</returns>
		public static T DeSerializer<T>(Stream xml) {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			object obj = xs.Deserialize(xml);

			return (T)obj;
		}
	}
}
