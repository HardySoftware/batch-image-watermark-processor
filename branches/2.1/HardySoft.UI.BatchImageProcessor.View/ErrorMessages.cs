using System.Collections.Generic;

namespace HardySoft.UI.BatchImageProcessor.View {
	public class ErrorMessages {
		/// <summary>
		/// Error messages related to one single operation.
		/// </summary>
		public List<string> ErrorMessageCollection {
			get;
			set;
		}

		/// <summary>
		/// To inticate if these errors are fatal.
		/// </summary>
		public bool FatalError {
			get;
			set;
		}
	}
}
