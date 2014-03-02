using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.CC.Mathematics {
	public enum CoordinateDirection {
		/// <summary>
		/// Latitude north.
		/// </summary>
		North,

		/// <summary>
		/// Latitude south.
		/// </summary>
		South,

		/// <summary>
		/// Longitude west.
		/// </summary>
		West,

		/// <summary>
		/// Longitude east.
		/// </summary>
		East,

		/// <summary>
		/// 0°0′0″ (at The Equator) is neither North nor South.
		/// </summary>
		TheEquator,

		/// <summary>
		/// A longitude of 0°0′0″ (at the Prime Meridian) is neither East nor West
		/// </summary>
		PrimeMeridian,
	}
}
