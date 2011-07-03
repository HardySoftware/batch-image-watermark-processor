using System;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model {
	[Serializable]
	public class BatchRename : INotifyPropertyChanged {
		//public event PropertyChangedEventHandler PropertyChanged;
		[NonSerialized]
		private PropertyChangedEventHandler propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged {
			add {
				propertyChanged += value;
			}
			remove {
				propertyChanged -= value;
			}
		}

		private void notify(string propName) {
			/*if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}*/

			if (propertyChanged != null) {
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		public BatchRename() {
			this.numberPadding = 3;
		}

		private bool enableBatchRename;
		public bool EnableBatchRename {
			get {
				return enableBatchRename;
			}
			set {
				if (this.enableBatchRename != value) {
					enableBatchRename = value;
					notify("EnableBatchRename");
				}
			}
		}

		private string outputFileNamePrefix;
		public string OutputFileNamePrefix {
			get {
				return outputFileNamePrefix;
			}
			set {
				if (string.Compare(this.outputFileNamePrefix, value, false) != 0) {
					outputFileNamePrefix = value;
					notify("OutputFileNamePrefix");
				}
			}
		}

		private string outputFileNameSuffix;
		public string OutputFileNameSuffix {
			get {
				return outputFileNameSuffix;
			}
			set {
				if (string.Compare(this.outputFileNameSuffix, value, false) != 0) {
					outputFileNameSuffix = value;
					notify("OutputFileNameSuffix");
				}
			}
		}

		private uint numberPadding;
		public uint NumberPadding {
			get {
				return numberPadding;
			}
			set {
				if (this.numberPadding != value) {
					numberPadding = value;
					notify("NumberPadding");
				}
			}
		}

		private uint startNumber;
		public uint StartNumber {
			get {
				return startNumber;
			}
			set {
				if (this.startNumber != value) {
					startNumber = value;
					notify("StartNumber");
				}
			}
		}

		private OutputFileNameCase fileNameCase;
		public OutputFileNameCase FileNameCase {
			get {
				return fileNameCase;
			}
			set {
				if (this.fileNameCase != value) {
					fileNameCase = value;
					notify("FileNameCase");
				}
			}
		}
	}
}
