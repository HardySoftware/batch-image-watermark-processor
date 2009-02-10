using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

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
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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