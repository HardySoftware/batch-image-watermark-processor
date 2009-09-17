using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace HardySoft.UI.BatchImageProcessor.View {
	/// <summary>
	/// A container for extra configuration (e.g. from command line and so on).
	/// </summary>
	public class ExtraConfiguration : IConfiguration {
		public CultureInfo ApplicationLanguage {
			get;
			set;
		}

		/*public bool EnableDebug {
			get;
			set;
		}*/
	}
}
