using System;

namespace HardySoft.CC.ExceptionLog {
	/// <summary>
	/// Exception containter class.
	/// </summary>
	[Serializable]
	public class ExceptionContainer {
		private KeyValuePairItem[] exceptionDetail;
		private CallStack[] stacks;

		/// <summary>
		/// Stack traces of lists method calls in order.
		/// </summary>
		public CallStack[] Stacks {
			get {
				return stacks;
			}
			set {
				stacks = value;
			}
		}

		/// <summary>
		/// Detailed exception information.
		/// </summary>
		public KeyValuePairItem[] ExceptionDetail {
			get {
				return exceptionDetail;
			}
			set {
				exceptionDetail = value;
			}
		}

		/// <summary>
		/// Type of exception.
		/// </summary>
		public string ExceptionType {
			get;
			set;
		}

		/// <summary>
		/// Indicates name of current thread.
		/// </summary>
		public string ThreadIdentity {
			get;
			set;
		}

		/// <summary>
		/// Application domain of the captured exception.
		/// </summary>
		/// <remarks>It is used to help to analyse where the exception is from.</remarks>
		public string ApplicationDomain {
			get;
			set;
		}

		/// <summary>
		/// Server's local time when exception is captured.
		/// </summary>
		public DateTime ExceptionTime {
			get;
			set;
		}

		/// <summary>
		/// Namespace, class name and method of which the error occurs.
		/// </summary>
		public string Source {
			get;
			set;
		}
	}

	/// <summary>
	/// Key/value pair item.
	/// </summary>
	[Serializable]
	public class KeyValuePairItem {
		private string key;
		private string value;

		/// <summary>
		/// Key name.
		/// </summary>
		public string Key {
			get {
				return key;
			}
			set {
				key = value;
			}
		}

		/// <summary>
		/// Value detail.
		/// </summary>
		public string Value {
			get {
				return this.value;
			}
			set {
				this.value = value;
			}
		}

		/// <summary>
		/// Initializes an instance of object without providing key/value information.
		/// </summary>
		public KeyValuePairItem() {
		}

		/// <summary>
		/// Initializes an instance of object with key/value information.
		/// </summary>
		/// <param name="key">Initial key.</param>
		/// <param name="value">Initial value.</param>
		public KeyValuePairItem(string key, string value) {
			this.key = key;
			this.value = value;
		}
	}

	/// <summary>
	/// Call stack item.
	/// </summary>
	[Serializable]
	public class CallStack {
		private int sequence;
		private string stack;

		/// <summary>
		/// Sequence of the call, smaller number means called earlier.
		/// </summary>
		public int Sequence {
			get {
				return sequence;
			}
			set {
				sequence = value;
			}
		}

		/// <summary>
		/// Stack trace item.
		/// </summary>
		public string Stack {
			get {
				return stack;
			}
			set {
				stack = value;
			}
		}
	}

}
