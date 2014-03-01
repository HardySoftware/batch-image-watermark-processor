
using System;
namespace HardySoft.UI.BatchImageProcessor.Model.Exif {
	/// <summary>
	/// A class used to represent a latitude or longitude data in GPS.
	/// </summary>
	public class GpsLocation {
		public GpsLocation(uint degree, uint? minute, float? second, CoordinateDirection coordinateDirection) {
			this.Degree = degree;
			this.Minute = minute;
			this.Second = second;
			this.CoordinateDirection = coordinateDirection;

			this.ConvertToDecimalDegree();
		}

		public GpsLocation(float decimalDegree, CoordinateType coordinateType) {
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