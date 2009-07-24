using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.View {
	public class ProcessThreadNumberEventArgs : EventArgs {
		private uint threadNumber;
		private string dateTimeFormat;

		public ProcessThreadNumberEventArgs(uint threadNumber, string dateTimeFormat)
			: base() {
			if (threadNumber == 0) {
				throw new ArgumentException("Please specify at least 1 thread.");
			} else {
				this.threadNumber = threadNumber;
			}

			this.dateTimeFormat = dateTimeFormat;
		}

		public uint ThreadNumber {
			get {
				return this.threadNumber;
			}
		}

		public string DateTimeFormat {
			get {
				return this.dateTimeFormat;
			}
		}
	}
}
