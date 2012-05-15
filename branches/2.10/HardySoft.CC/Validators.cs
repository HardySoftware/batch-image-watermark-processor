using System.Text.RegularExpressions;

namespace HardySoft.CC.Validation {
	/// <summary>
	/// Class to provide functions to validate against multiple purposes
	/// </summary>
	public class Validators {
		/// <summary>
		/// Check if the string is in integer format
		/// </summary>
		/// <param name="inputValue">String to check</param>
		/// <returns>true or false</returns>
		public static bool IsInteger(string inputValue) {
			if (inputValue == null) {
				return false;
			} else {
				if (inputValue.Trim() == "") {
					return false;
				}
			}

			if (Regex.IsMatch(inputValue.Trim(), @"^[-+]?[0-9][0-9]{0,}$")) {
				return true;
			} else {
				return false;
			}
		}
	}
}
