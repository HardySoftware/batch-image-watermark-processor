using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// Defines types for Exif Properties.
	/// </summary>
	/// <remarks>
	/// // property type http://msdn.microsoft.com/en-us/library/xddt0dz7(v=vs.110).aspx
	/// </remarks>
	internal enum ExifType {
		/// <summary>
		/// Specifies that the type is either unknown or not defined.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Specifies that the value buffer member is an array of bytes.
		/// </summary>
		Byte = 1,

		/// <summary>
		/// Specifies that the value buffer member is a null-terminated ASCII string.
		/// </summary>
		/// <remarks>If you set <see cref="ExifProperty.Type">ExifProperty.Type</see> to <see cref="ExifProperty.Type"/>, you should set the length buffer member to the length of the string including the NULL terminator. For example, the string HELLO would have a length of 6.</remarks>
		Ascii = 2,

		/// <summary>
		/// Specifies that the value buffer member is an array of signed short (16-bit) integers.
		/// </summary>
		UInt16 = 3,

		/// <summary>
		/// Specifies that the value buffer member is an array of unsigned long (32-bit) integers.
		/// </summary>
		UInt32 = 4,

		/// <summary>
		/// Specifies that the value buffer member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
		/// </summary>
		URational = 5,

		/// <summary>
		/// Specifies that the value buffer member is an array of bytes that can hold values of any buffer type.
		/// </summary>
		Raw = 7,

		/// <summary>
		/// Specifies that the value buffer member is an array of signed long (32-bit) integers.
		/// </summary>
		Int32 = 9,

		/// <summary>
		/// Specifies that the value buffer member is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
		/// </summary>
		Rational = 10
	}
}
