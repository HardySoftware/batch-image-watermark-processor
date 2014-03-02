using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// Describes the operations which need to be performed to display the image.
	/// Alternatively the orientation of the camera when the image was captured.
	/// </summary>
	/// <remarks>
	/// All degrees are given as clockwise.
	/// </remarks>
	public enum ExifOrientation : ushort {
		/* EXIF orientation example from Adam M. Costello
		 * 
		 * Here is what the letter F would look like if it were tagged correctly and displayed
		 * by a program that ignores the orientation tag (thus showing the stored image):
		 * 
		 *   1        2       3      4         5            6           7          8
		 * 
		 * 888888  888888      88  88      8888888888  88                  88  8888888888
		 * 88          88      88  88      88  88      88  88          88  88      88  88
		 * 8888      8888    8888  8888    88          8888888888  8888888888          88
		 * 88          88      88  88
		 * 88          88  888888  888888
		*/

		[Description("Unknown")]
		Unknown = 0,

		[Description("Normal")]
		Normal = 1,

		[Description("Flip Horizontal")]
		FlipHorizontal = 2,

		[Description("Rotate 180")]
		Rotate180 = 3,

		[Description("Flip Vertical")]
		FlipVertical = 4,

		[Description("Rotate 90 Flip Horizontal")]
		Rotate90FlipHorizontal = 5,

		[Description("Rotate 90")]
		Rotate90 = 6,

		[Description("Rotate 270 Flip Horizontal")]
		Rotate270FlipHorizontal = 7,

		[Description("Rotate 270")]
		Rotate270 = 8
	}
}
