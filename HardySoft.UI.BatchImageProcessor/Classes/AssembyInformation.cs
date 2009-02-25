using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;
using System.IO;

namespace HardySoft.UI.BatchImageProcessor.Classes {
	public class AssembyInformation {
		public string AssemblyTitle {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0) {
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "") {
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion {
			get {
				try {
					return GetApplicationVersion().ToString();
				} catch {
					return string.Empty;
				}
			}
		}

		public string AssemblyDescription {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		public List<ReferencedAssembly> ReferencedAssemblies {
			get {
				List<ReferencedAssembly> referenced = new List<ReferencedAssembly>();

				Assembly entryAssembly = Assembly.GetEntryAssembly();
				foreach (AssemblyName name in entryAssembly.GetReferencedAssemblies()) {
					Console.WriteLine("Name: {0}", name.ToString());
					ReferencedAssembly ra = ReferencedAssembly.ParseAssmbly(name);

					if (ra != null) {
						referenced.Add(ra);
					}
				}

				return referenced;
			}
		}

		public string ApplicationUrl {
			get {
				return Resources.LanguageContent.ApplicationUrl;
			}
		}

		/// <summary>
		/// Get latest version available online.
		/// </summary>
		/// <returns>
		/// Latest version number, or null if any exception happens.
		/// </returns>
		public static Version GetLatestVersion() {
			try {
				WebRequest request = WebRequest.Create("http://code.google.com/p/batch-image-watermark-processor/wiki/CurrentVersion");
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();

				Regex reg = new Regex(@"&gt;&gt;&gt;([\d]+\.[\d]+\.[\d]+\.[\d]+)?&lt;&lt;&lt;");
				MatchCollection matches = reg.Matches(responseFromServer);

				Version v = null;

				if (matches != null) {
					for (int i = 0; i < matches.Count; i++) {
						Match m = matches[i];
						string foundVersion = m.Value;
						foundVersion = foundVersion.Replace("&gt;", "");
						foundVersion = foundVersion.Replace(">", "");
						foundVersion = foundVersion.Replace("&lt;", "");
						foundVersion = foundVersion.Replace("<", "");

						v = new Version(foundVersion);
					}
				}

				return v;
			} catch {
				return null;
			}
		}

		/// <summary>
		/// Get current application's version number.
		/// </summary>
		/// <returns></returns>
		public static Version GetApplicationVersion() {
			return Assembly.GetExecutingAssembly().GetName().Version;
		}
	}

	public class ReferencedAssembly {
		public static ReferencedAssembly ParseAssmbly(AssemblyName assemblyName) {
			ReferencedAssembly ra = new ReferencedAssembly();
			ra.Name = assemblyName.Name;
			ra.Version = assemblyName.Version.ToString();
			return ra;
		}

		public string Name {
			get;
			set;
		}

		public string Version {
			get;
			set;
		}
	}
}