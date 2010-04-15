using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.View {
	public class OneErrorMessage {
		/// <summary>
		/// Exception object related to one single operation
		/// </summary>
		public Exception Error {
			get;
			set;
		}

		/// <summary>
		/// To inticate if this error is fatal.
		/// </summary>
		public bool FatalError {
			get;
			set;
		}
	}
}
