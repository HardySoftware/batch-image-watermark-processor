using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.CC.Mathematics {
	/// <summary>
	/// A class used to represent a latitude or longitude data in geographic coordinate.
	/// </summary>
	public class GeographicCoordinate {
		public GeographicCoordinate(uint degree, uint? minute, float? second, CoordinateDirection coordinateDirection) {
			if (coordinateDirection == CoordinateDirection.North
				|| coordinateDirection == CoordinateDirection.South
				|| coordinateDirection == CoordinateDirection.TheEquator) {
					if (degree > 90) {
						throw new ArgumentException("Degree must be between 0 and 90.", "degree");
					}
			} else if (coordinateDirection == CoordinateDirection.East
				|| coordinateDirection == CoordinateDirection.West
				|| coordinateDirection == CoordinateDirection.PrimeMeridian) {
					if (degree > 180) {
						throw new ArgumentException("Degree must be between 0 and 180.", "degree");
					}
			}

			if (minute.HasValue && minute.Value >= 60) {
				throw new ArgumentException("Minute must be between 0 and 60.", "minute");
			}

			if (second.HasValue && second.Value >= 60) {
				throw new ArgumentException("Second must be between 0 and 60.", "second");
			}

			this.Degree = degree;
			this.Minute = minute;
			this.Second = second;
			this.CoordinateDirection = coordinateDirection;

			this.ConvertToDecimalDegree();
		}

		public GeographicCoordinate(float decimalDegree, CoordinateType coordinateType) {
			if (coordinateType == CoordinateType.Latitude) {
				if (decimalDegree > 90 || decimalDegree < -90) {
					throw new ArgumentException("Invalid latitude number.");
				}
			} else if (coordinateType == CoordinateType.Longitude) {
				if (decimalDegree > 180 || decimalDegree < -180) {
					throw new ArgumentException("Invalid longitude number.");
				}
			}
			this.DecimalDegree = decimalDegree;
			this.CoordinateType = coordinateType;

			this.ConvertToDMS();
		}

		#region DMS representation
		public uint Degree { get; private set; }

		public uint? Minute { get; private set; }

		public float? Second { get; private set; }

		public CoordinateDirection CoordinateDirection { get; private set; }
		#endregion

		#region Decimal degree representation
		public float DecimalDegree { get; private set; }

		public CoordinateType CoordinateType { get; private set; }
		#endregion

		/// <summary>
		/// Convert a Decimal to Sexagesimal.
		/// </summary>
		private void ConvertToDMS() {
			if (this.DecimalDegree > 0 && this.CoordinateType == CoordinateType.Latitude) {
				this.CoordinateDirection = CoordinateDirection.North;
			} else if (this.DecimalDegree < 0 && this.CoordinateType == CoordinateType.Latitude) {
				this.CoordinateDirection = CoordinateDirection.South;
			} else if (this.DecimalDegree == 0 && this.CoordinateType == CoordinateType.Latitude) {
				this.CoordinateDirection = CoordinateDirection.TheEquator;
			} else if (this.DecimalDegree > 0 && this.CoordinateType == CoordinateType.Longitude) {
				this.CoordinateDirection = CoordinateDirection.East;
			} else if (this.DecimalDegree < 0 && this.CoordinateType == CoordinateType.Longitude) {
				this.CoordinateDirection = CoordinateDirection.West;
			} else if (this.DecimalDegree == 0 && this.CoordinateType == CoordinateType.Longitude) {
				this.CoordinateDirection = CoordinateDirection.PrimeMeridian;
			}

			float decimalAdgree = Math.Abs(this.DecimalDegree);

			this.Degree = Convert.ToUInt32(this.DecimalDegree);

			float remainder = this.DecimalDegree - this.Degree;

			this.Minute = Convert.ToUInt32(remainder * 60);

			remainder = remainder - this.Minute.Value;

			this.Second = remainder;
		}

		/// <summary>
		/// 
		/// </summary>
		private void ConvertToDecimalDegree() {
			uint minute = this.Minute.HasValue ? this.Minute.Value : 0;
			float second = this.Second.HasValue ? this.Second.Value : 0;

			this.DecimalDegree = this.Degree + (minute * 60 + second) / 3600;

			if (this.CoordinateDirection == CoordinateDirection.West || this.CoordinateDirection == CoordinateDirection.South) {
				this.DecimalDegree *= -1;
			}

			switch (this.CoordinateDirection) {
				case CoordinateDirection.East:
				case CoordinateDirection.West:
				case CoordinateDirection.PrimeMeridian:
					this.CoordinateType = CoordinateType.Longitude;
					break;
				case CoordinateDirection.North:
				case CoordinateDirection.South:
				case CoordinateDirection.TheEquator:
					this.CoordinateType = CoordinateType.Latitude;
					break;
			}
		}
	}
}
