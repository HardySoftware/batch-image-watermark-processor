using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;
using System.IO;
using System.Globalization;
using System.Linq;

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
				AssemblyName root = Assembly.GetEntryAssembly().GetName();
				walkThroughAssemblies(root);
				
				referenced.Add(ReferencedAssembly.ParseAssmbly(root));
				return referenced;
			}
		}

		List<ReferencedAssembly> referenced = new List<ReferencedAssembly>();

		private void walkThroughAssemblies(AssemblyName an) {
			Assembly assembly;
			try {
				assembly = Assembly.Load(an);
			} catch {
				// oops
				return;
			}

			foreach (AssemblyName a in assembly.GetReferencedAssemblies()) {
				ReferencedAssembly ra = ReferencedAssembly.ParseAssmbly(a);
				if (ra != null && referenced.FirstOrDefault(r => r.Name == ra.Name && r.Version == ra.Version) == null) {
					referenced.Add(ra);
				}
			}

			// see what is referenced
			foreach (AssemblyName nextRef in assembly.GetReferencedAssemblies()) {
				if (nextRef.FullName.StartsWith("HardySoft")) {
					walkThroughAssemblies(nextRef);
				}
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
                WebRequest request = WebRequest.Create("https://raw.githubusercontent.com/HardySoftware/batch-image-watermark-processor/wiki/CurrentVersion.md");
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();

                Regex reg = new Regex(@"{Current Version Begins}([\d]+\.[\d]+\.[\d]+\.[\d]+)?{Current Version Ends}");
				MatchCollection matches = reg.Matches(responseFromServer);

				Version v = null;

				if (matches != null) {
					for (int i = 0; i < matches.Count; i++) {
						Match m = matches[i];
						string foundVersion = m.Value;
                        foundVersion = foundVersion.Replace("{Current Version Begins}", "");
                        foundVersion = foundVersion.Replace("{Current Version Ends}", "");

						v = new Version(foundVersion);
					}
				}

				return v;
			} catch (Exception ex) {
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