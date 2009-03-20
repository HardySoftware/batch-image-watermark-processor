using System;
using System.Text;

namespace HardySoft.CC.Converter {
	public class StringConverter {
		/// <summary>
		/// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
		/// </summary>
		/// <param name="byteArray">Unicode Byte Array to be converted to String</param>
		/// <returns>String converted from Unicode Byte Array</returns>
		public static String UTF8ByteArrayToString(Byte[] byteArray) {
			UTF8Encoding encoding = new UTF8Encoding();
			String constructedString = encoding.GetString(byteArray);
			return (constructedString);
		}

		/// <summary>
		/// Converts the String to UTF8 Byte array and is used in De serialization
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Byte[] StringToUTF8ByteArray(String text) {
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(text);
			return byteArray;
		} 
	}
}