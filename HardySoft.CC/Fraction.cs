using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace HardySoft.CC.Mathematics {
	/// <summary>
	/// Class to represent Fraction.
	/// </summary>
	/// <remarks>
	/// Original article from http://www.codeproject.com/KB/recipes/fractiion.aspx
	/// </remarks>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public class Fraction : IComparable, IFormattable {
		private long numerator;
		private long denominator;

		/// <summary>
		/// The 'top' part of the fraction.
		/// </summary>
		/// <remarks>
		/// For 3/4ths, this is the 3.
		/// </remarks>
		public long Numerator {
			get {
				return numerator;
			}
			set {
				numerator = value;
			}
		}

		/// <summary>
		/// The 'bottom' part of the fraction.
		/// </summary>
		/// <remarks>
		/// For 3/4ths, this is the 4
		/// </remarks>
		public long Denominator {
			get {
				return denominator;
			}
			set {
				denominator = value;
			}
		}

		#region Constructors
		/// <summary>
		/// Construct a Fraction from an integral value
		/// </summary>
		/// <param name="wholeNumber">
		/// The value (eventual numerator).
		/// </param>
		/// <remarks>
		/// The denominator will be 1.
		/// </remarks>
		public Fraction(long wholeNumber) {
			if (wholeNumber == long.MinValue) {
				wholeNumber++;
			}
			// prevent serious issues later..
			this.numerator = wholeNumber;
			this.denominator = 1; // no reducing required, we're a whole number
		}

		/// <summary>
		/// Construct a Fraction from a floating-point value.
		/// </summary>
		/// <param name="floatingPointNumber">
		/// The value.
		/// </param>
		public Fraction(double floatingPointNumber) {
			Fraction f = ToFraction(floatingPointNumber);
			this.denominator = f.Denominator;
			this.numerator = f.Numerator;
		}

		/// <summary>
		/// Construct a Fraction from a string in any legal format.
		/// </summary>
		/// <param name="inValue">
		/// A string with a legal fraction input format.
		/// </param>
		/// <param name="info">Number info used to parse string.</param>
		/// <remarks>
		/// Will reduce the fraction to smallest possible denominator
		/// ToFraction(string strValue).
		/// </remarks>
		public Fraction(string inValue, NumberFormatInfo info) {
			Fraction frac = ToFraction(inValue, info);
			this.Numerator = frac.Numerator;
			this.Denominator = frac.Denominator;
		}

		/// <summary>
		/// Construct a Fraction from a string in any legal format.
		/// </summary>
		/// <param name="inValue">
		/// A string with a legal fraction input format.
		/// </param>
		/// <remarks>
		/// The current culture's number format information will be used.
		/// </remarks>
		public Fraction(string inValue) {
			NumberFormatInfo info = NumberFormatInfo.CurrentInfo;
			Fraction frac = ToFraction(inValue, info);
			this.Numerator = frac.Numerator;
			this.Denominator = frac.Denominator;
		}

		/// <summary>
		/// Construct a Fraction from a numerator, denominator pair.
		/// </summary>
		/// <param name="numerator">
		/// The numerator (top number).
		/// </param>
		/// <param name="denominator">
		/// The denominator (bottom number).
		/// </param>
		/// <remarks>
		/// Will reduce the fraction to smallest possible denominator.
		/// </remarks>
		public Fraction(long numerator, long denominator) {
			if (numerator == long.MinValue) {
				numerator++;
				// prevent serious issues later..
			}
			if (denominator == long.MinValue) {
				denominator++;
				// prevent serious issues later..
			}
			this.numerator = numerator;
			this.denominator = denominator;
			ReduceFraction();
		}

		/// <summary>
		/// Private constructor to synthesize a Fraction for indeterminates (NaN and infinites).
		/// </summary>
		/// <param name="type">
		/// Kind of inderterminate
		/// </param>
		private Fraction(Indeterminates type) {
			this.numerator = (long)type;
			this.denominator = 0;
			// do NOT reduce, we're clean as can be!
		}
		#endregion

		#region Expose constants
		/// <summary>
		/// Represents zero denominator fraction.
		/// </summary>
		public static readonly Fraction NaN = new Fraction(Indeterminates.NaN);

		/// <summary>
		/// Represents positive infinity fraction.
		/// </summary>
		public static readonly Fraction PositiveInfinity = new Fraction(Indeterminates.PositiveInfinity);

		/// <summary>
		/// Represents negative infinity fraction.
		/// </summary>
		public static readonly Fraction NegativeInfinity = new Fraction(Indeterminates.NegativeInfinity);

		/// <summary>
		/// Represents zero as fraction.
		/// </summary>
		public static readonly Fraction Zero = new Fraction(0, 1);

		/// <summary>
		/// Represents epsilon as fraction.
		/// </summary>
		public static readonly Fraction Epsilon = new Fraction(1, Int64.MaxValue);

		/// <summary>
		/// Represents epsilon as double.
		/// </summary>
		private static readonly double EpsilonDouble = 1.0 / Int64.MaxValue;

		/// <summary>
		/// Represents maximum value as fraction.
		/// </summary>
		public static readonly Fraction MaxValue = new Fraction(Int64.MaxValue, 1);

		/// <summary>
		/// Represents minimum value as fraction.
		/// </summary>
		public static readonly Fraction MinValue = new Fraction(Int64.MinValue, 1);
		#endregion

		#region Explicit conversions
		#region To primitives
		/// <summary>
		/// Get the integral value of the Fraction object as int/Int32
		/// </summary>
		/// <returns>
		/// The (approximate) integer value.
		/// If the value is not a true integer, the fractional part is discarded
		/// (truncated toward zero). If the valid exceeds the range of an Int32 and exception is thrown.
		/// Will throw a FractionException for NaN, PositiveInfinity
		/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.
		/// large or small to be represented as an Int32.
		/// </returns>
		public Int32 ToInt32() {
			if (this.denominator == 0) {
				throw new FractionException(string.Format("Cannot convert {0} to Int32",
					indeterminateTypeName(this.numerator)),
					new System.NotFiniteNumberException());
			}

			long bestGuess = this.numerator / this.denominator;
			if (bestGuess > Int32.MaxValue || bestGuess < Int32.MinValue) {
				throw new FractionException("Cannot convert to Int32", new System.OverflowException());
			}

			return (Int32)bestGuess;
		}

		/// <summary>
		/// Get the integral value of the Fraction object as long/Int64.
		/// </summary>
		/// <returns>
		/// The (approximate) integer value.
		/// If the value is not a true integer, the fractional part is discarded
		/// (truncated toward zero). If the valid exceeds the range of an Int32, no special
		/// handling is guaranteed.
		/// Will throw a FractionException for NaN, PositiveInfinity
		/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.
		/// </returns>
		public Int64 ToInt64() {
			if (this.denominator == 0) {
				throw new FractionException(string.Format("Cannot convert {0} to Int64",
					indeterminateTypeName(this.numerator)),
					new System.NotFiniteNumberException());
			}

			return this.numerator / this.denominator;
		}

		/// <summary>
		/// Get the value of the Fraction object as double with full support for NaNs and infinities.
		/// </summary>
		/// <returns>
		/// The decimal representation of the Fraction, or double.NaN, double.NegativeInfinity
		/// or double.PositiveInfinity.
		/// </returns>
		public double ToDouble() {
			if (this.denominator == 1) {
				return this.numerator;
			} else if (this.denominator == 0) {
				switch (NormalizeIndeterminate(this.numerator)) {
					case Indeterminates.NegativeInfinity:
						return double.NegativeInfinity;
					case Indeterminates.PositiveInfinity:
						return double.PositiveInfinity;
					case Indeterminates.NaN:
					default:
						// this can't happen
						return double.NaN;
				}
			} else {
				return (double)this.numerator / (double)this.denominator;
			}
		}

		/// <summary>
		/// Get the value of the Fraction as a string, with proper representation for NaNs and infinites.
		/// </summary>
		/// <returns>
		/// The string representation of the Fraction, or the culture-specific representations of
		/// NaN, PositiveInfinity or NegativeInfinity.
		/// The current culture determines the textual representation the Indeterminates.
		/// </returns>
		public override string ToString() {
			if (this.denominator == 1) {
				return this.numerator.ToString();
			} else if (this.denominator == 0) {
				return indeterminateTypeName(this.numerator);
			} else {
				if (this.numerator > this.denominator) {
					long div = this.numerator / this.denominator;
					Fraction rem = new Fraction(this.numerator - (div * this.denominator), this.denominator);
					return div.ToString() + " " + rem.ToString();
				} else {
					return this.numerator.ToString() + "/" + this.denominator.ToString();
				}
			}
		}
		#endregion

		#region From primitives
		/// <summary>
		/// Converts a long value to the exact Fraction.
		/// </summary>
		/// <param name="inValue">The long to convert.</param>
		/// <returns>An exact representation of the value.</returns>
		public static Fraction ToFraction(long inValue) {
			return new Fraction(inValue);
		}

		/// <summary>
		/// Converts a double value to the approximate Fraction.
		/// </summary>
		/// <param name="inValue">The double to convert.</param>
		/// <returns>
		/// A best-fit representation of the value.
		/// Supports double.NaN, double.PositiveInfinity and double.NegativeInfinity.
		/// </returns>
		public static Fraction ToFraction(double inValue) {
			// it's one of the indeterminates... which?				
			if (double.IsNaN(inValue)) {
				return NaN;
			} else if (double.IsNegativeInfinity(inValue)) {
				return NegativeInfinity;
			} else if (double.IsPositiveInfinity(inValue)) {
				return PositiveInfinity;
			} else if (inValue == 0.0d) {
				return Zero;
			}

			if (inValue > Int64.MaxValue) {
				throw new OverflowException(string.Format("Double {0} too large", inValue));
			}
			if (inValue < -Int64.MaxValue) {
				throw new OverflowException(string.Format("Double {0} too small", inValue));
			}
			if (-EpsilonDouble < inValue && inValue < EpsilonDouble) {
				throw new ArithmeticException(string.Format("Double {0} cannot be represented", inValue));
			}
			int sign = Math.Sign(inValue);
			inValue = Math.Abs(inValue);
			return convertPositiveDouble(sign, inValue);
		}

		/// <summary>
		/// Converts a string to the corresponding reduced fraction.
		/// </summary>
		/// <param name="inValue">The string representation of a fractional value.</param>
		/// <param name="info">Number info to parse number string.</param>
		/// <returns>
		/// The Fraction that represents the string.
		/// Four forms are supported, as a plain integer, as a double, or as Numerator/Denominator
		/// and the representations for NaN and the infinites.
		/// "123" = 123/1 and "1.25" = 5/4 and "10/36" = 5/13 and NaN = 0/0 and
		/// PositiveInfinity = 1/0 and NegativeInfinity = -1/0.
		/// </returns>
		public static Fraction ToFraction(string inValue, NumberFormatInfo info) {
			if (inValue == null || inValue == string.Empty) {
				throw new ArgumentNullException("inValue");
			}

			// could also be NumberFormatInfo.InvariantInfo	
			// NumberFormatInfo info = NumberFormatInfo.CurrentInfo;
			// Is it one of the special symbols for NaN and such...	
			string trimmedValue = inValue.Trim();
			if (trimmedValue == info.NaNSymbol) {
				return NaN;
			} else if (trimmedValue == info.PositiveInfinitySymbol) {
				return PositiveInfinity;
			} else if (trimmedValue == info.NegativeInfinitySymbol) {
				return NegativeInfinity;
			} else {
				// Not special, is it a Fraction?
				int slashPos = inValue.IndexOf('/');
				if (slashPos > -1) {
					// string is in the form of Numerator/Denominator
					long numerator = Convert.ToInt64(inValue.Substring(0, slashPos));
					long denominator = Convert.ToInt64(inValue.Substring(slashPos + 1));
					return new Fraction(numerator, denominator);
				} else {
					// the string is not in the form of a fraction
					// hopefully it is double or integer, do we see a decimal point?
					int decimalPos = inValue.IndexOf(info.CurrencyDecimalSeparator);
					if (decimalPos > -1) {
						// TODO this piece looks week, it should consider CurrencyGroupSeparator and so on.
						return new Fraction(Convert.ToDouble(inValue));
					} else {
						// TODO this piece looks week, it should consider CurrencyGroupSeparator and so on.
						return new Fraction(Convert.ToInt64(inValue));
					}
				}
			}
		}
		#endregion
		#endregion

		#region Indeterminate classifications
		/// <summary>
		/// Determines if a Fraction represents a Not-a-Number
		/// </summary>
		/// <returns>
		/// True if the Fraction is a NaN.
		/// </returns>
		public bool IsNaN() {
			if (this.denominator == 0 && NormalizeIndeterminate(this.numerator) == Indeterminates.NaN) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Determines if a Fraction represents Any Infinity.
		/// </summary>
		/// <returns>
		/// True if the Fraction is Positive Infinity or Negative Infinity.
		/// </returns>
		public bool IsInfinity() {
			if (this.denominator == 0 && NormalizeIndeterminate(this.numerator) != Indeterminates.NaN) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Determines if a Fraction represents Positive Infinity.
		/// </summary>
		/// <returns>True if the Fraction is Positive Infinity.</returns>
		public bool IsPositiveInfinity() {
			if (this.denominator == 0 && NormalizeIndeterminate(this.numerator) == Indeterminates.PositiveInfinity) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Determines if a Fraction represents Negative Infinity.
		/// </summary>
		/// <returns>True if the Fraction is Negative Infinity.</returns>
		public bool IsNegativeInfinity() {
			if (this.denominator == 0 && NormalizeIndeterminate(this.numerator) == Indeterminates.NegativeInfinity) {
				return true;
			} else {
				return false;
			}
		}
		#endregion

		#region Inversion
		/// <summary>
		/// Inverts a Fraction.
		/// </summary>
		/// <remarks>
		/// Does NOT throw for zero Numerators as later use of the fraction will catch the error.
		/// </remarks>
		public void Inverse() {
			long temp;
			// don't use the obvious constructor because we do not want it normalized at this time
			temp = this.denominator;
			this.denominator = this.numerator;
			this.numerator = temp;
		}

		/// <summary>
		/// Creates an inverted Fraction.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>The inverted Fraction (with Denominator over Numerator).</returns>
		/// <remarks>
		/// Does NOT throw for zero Numerators as later use of the fraction will catch the error.
		/// </remarks>
		public static Fraction Inverted(long value) {
			Fraction frac = new Fraction(value);
			frac.Inverse();
			return frac;
		}

		/// <summary>
		/// Creates an inverted Fraction.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>The inverted Fraction (with Denominator over Numerator).</returns>
		/// <remarks>
		/// Does NOT throw for zero Numerators as later use of the fraction will catch the error.
		/// </remarks>
		public static Fraction Inverted(double value) {
			Fraction frac = new Fraction(value);
			frac.Inverse();
			return frac;
		}
		#endregion

		#region Operators
		#region Unary Negation operator
		/// <summary>
		/// Negates the Fraction.
		/// </summary>
		/// <param name="left">The Fraction to negate.</param>
		/// <returns>The negative version of the Fraction.</returns>
		public static Fraction operator -(Fraction left) {
			return negate(left);
		}
		#endregion

		#region Addition operators
		public static Fraction operator +(Fraction left, Fraction right) {
			return add(left, right);
		}

		public static Fraction operator +(long left, Fraction right) {
			return add(new Fraction(left), right);
		}

		public static Fraction operator +(Fraction left, long right) {
			return add(left, new Fraction(right));
		}

		public static Fraction operator +(double left, Fraction right) {
			return add(ToFraction(left), right);
		}

		public static Fraction operator +(Fraction left, double right) {
			return add(left, ToFraction(right));
		}
		#endregion

		#region Subtraction operators
		public static Fraction operator -(Fraction left, Fraction right) {
			return add(left, -right);
		}

		public static Fraction operator -(long left, Fraction right) {
			return add(new Fraction(left), -right);
		}

		public static Fraction operator -(Fraction left, long right) {
			return add(left, new Fraction(-right));
		}

		public static Fraction operator -(double left, Fraction right) {
			return add(ToFraction(left), -right);
		}

		public static Fraction operator -(Fraction left, double right) {
			return add(left, ToFraction(-right));
		}
		#endregion

		#region Multiplication operators
		public static Fraction operator *(Fraction left, Fraction right) {
			return multiply(left, right);
		}

		public static Fraction operator *(long left, Fraction right) {
			return multiply(new Fraction(left), right);
		}

		public static Fraction operator *(Fraction left, long right) {
			return multiply(left, new Fraction(right));
		}

		public static Fraction operator *(double left, Fraction right) {
			return multiply(ToFraction(left), right);
		}

		public static Fraction operator *(Fraction left, double right) {
			return multiply(left, ToFraction(right));
		}
		#endregion

		#region Division operators
		public static Fraction operator /(Fraction left, Fraction right) {
			right.Inverse();
			return multiply(left, right);
		}

		public static Fraction operator /(long left, Fraction right) {
			right.Inverse();
			return multiply(new Fraction(left), right);
		}

		public static Fraction operator /(Fraction left, long right) {
			return multiply(left, Inverted(right));
		}

		public static Fraction operator /(double left, Fraction right) {
			right.Inverse();
			return multiply(ToFraction(left), right);
		}

		public static Fraction operator /(Fraction left, double right) {
			return multiply(left, Inverted(right));
		}
		#endregion

		#region modulus operators
		public static Fraction operator %(Fraction left, Fraction right) {
			return modulus(left, right);
		}

		public static Fraction operator %(long left, Fraction right) {
			return modulus(new Fraction(left), right);
		}

		public static Fraction operator %(Fraction left, long right) {
			return modulus(left, right);
		}

		public static Fraction operator %(double left, Fraction right) {
			return modulus(ToFraction(left), right);
		}

		public static Fraction operator %(Fraction left, double right) {
			return modulus(left, right);
		}
		#endregion

		#region Equal operators
		public static bool operator ==(Fraction left, Fraction right) {
			if ((object)left == null && (object)right == null) {
				return true;
			} else if ((object)left == null || (object)right == null) {
				return false;
			} else {
				return left.compareEquality(right, false);
			}
		}

		public static bool operator ==(Fraction left, long right) {
			if ((object)left == null) {
				return false;
			} else {
				return left.compareEquality(new Fraction(right), false);
			}
		}

		public static bool operator ==(Fraction left, double right) {
			if ((object)left == null) {
				return false;
			} else {
				return left.compareEquality(new Fraction(right), false);
			}
		}
		#endregion

		#region Not-equal operators
		public static bool operator !=(Fraction left, Fraction right) {
			if ((object)left == null && (object)right == null) {
				return false;
			} else if ((object)left == null || (object)right == null) {
				return true;
			} else {
				return left.compareEquality(right, true);
			}
		}

		public static bool operator !=(Fraction left, long right) {
			if ((object)left == null) {
				return true;
			} else {
				return left.compareEquality(new Fraction(right), true);
			}
		}

		public static bool operator !=(Fraction left, double right) {
			if ((object)left == null) {
				return true;
			} else {
				return left.compareEquality(new Fraction(right), true);
			}
		}
		#endregion

		#region Inequality operators
		/// <summary>
		/// Compares two Fractions to see if left is less than right.
		/// </summary>
		/// <param name="left">The first Fraction.</param>
		/// <param name="right">The second Fraction.</param>
		/// <returns>
		/// True if left is less than right.
		/// </returns>
		/// <remarks>
		/// Special handling for indeterminates exists. IndeterminateLess
		/// Throws an error if overflows occur while computing the 
		/// difference with an InnerException of OverflowException.
		/// </remarks>
		public static bool operator <(Fraction left, Fraction right) {
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Compares two Fractions to see if left is greater than right.
		/// </summary>
		/// <param name="left">The first Fraction.</param>
		/// <param name="right">The second Fraction.</param>
		/// <returns>
		/// True if left is greater than right.
		/// </returns>
		/// <remarks>
		/// Special handling for indeterminates exists. IndeterminateLess
		/// Throws an error if overflows occur while computing the
		/// difference with an InnerException of OverflowException.
		/// </remarks>
		public static bool operator >(Fraction left, Fraction right) {
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Compares two Fractions to see if left is less than or equal to right.
		/// </summary>
		/// <param name="left">The first Fraction.</param>
		/// <param name="right">The second Fraction.</param>
		/// <returns>
		/// True if left is less than or equal to right.
		/// </returns>
		/// <remarks>
		/// Special handling for indeterminates exists. IndeterminateLessEqual
		/// Throws an error if overflows occur while computing the
		/// difference with an InnerException of OverflowException.
		/// </remarks>
		public static bool operator <=(Fraction left, Fraction right) {
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Compares two Fractions to see if left is greater than or equal to right.
		/// </summary>
		/// <param name="left">The first Fraction.</param>
		/// <param name="right">The second Fraction.</param>
		/// <returns>
		/// True if left is greater than or equal to right.
		/// </returns>
		/// <remarks>
		/// Special handling for indeterminates exists. IndeterminateLessEqual
		/// Throws an error if overflows occur while computing the
		/// difference with an InnerException of OverflowException.
		/// </remarks>
		public static bool operator >=(Fraction left, Fraction right) {
			return left.CompareTo(right) >= 0;
		}
		#endregion

		#region Implict conversion from primitive operators
		/// <summary>
		/// Implicit conversion of a long integral value to a Fraction.
		/// </summary>
		/// <param name="value">The long integral value to convert.</param>
		/// <returns>A Fraction whose denominator is 1.</returns>
		public static implicit operator Fraction(long value) {
			return new Fraction(value);
		}

		/// <summary>
		/// Implicit conversion of a double floating point value to a Fraction.
		/// </summary>
		/// <param name="value">The double value to convert.</param>
		/// <returns>A reduced Fraction.</returns>
		public static implicit operator Fraction(double value) {
			return new Fraction(value);
		}

		/// <summary>
		/// Implicit conversion of a string to a Fraction.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>A reduced Fraction</returns>
		/// <remarks>Current culture's number info will be used to parse string.</remarks>
		public static implicit operator Fraction(string value) {
			return new Fraction(value);
		}
		#endregion

		#region Explicit converstion to primitive operators
		/// <summary>
		/// Explicit conversion from a Fraction to an integer.
		/// </summary>
		/// <param name="frac">the Fraction to convert.</param>
		/// <returns>The integral representation of the Fraction.</returns>
		public static explicit operator int(Fraction frac) {
			return frac.ToInt32();
		}

		/// <summary>
		/// Explicit conversion from a Fraction to an integer.
		/// </summary>
		/// <param name="frac">The Fraction to convert.</param>
		/// <returns>The integral representation of the Fraction.</returns>
		public static explicit operator long(Fraction frac) {
			return frac.ToInt64();
		}

		/// <summary>
		/// Explicit conversion from a Fraction to a double floating-point value.
		/// </summary>
		/// <param name="frac">The Fraction to convert.</param>
		/// <returns>The double representation of the Fraction.</returns>
		public static explicit operator double(Fraction frac) {
			return frac.ToDouble();
		}

		/// <summary>
		/// Explicit conversion from a Fraction to a string.
		/// </summary>
		/// <param name="frac">the Fraction to convert.</param>
		/// <returns>The string representation of the Fraction.</returns>
		public static implicit operator string(Fraction frac) {
			return frac.ToString();
		}
		#endregion
		#endregion

		#region Equals and GetHashCode overrides
		/// <summary>
		/// Compares for equality the current Fraction to the value passed.
		/// </summary>
		/// <param name="obj">A Fraction.</param>
		/// <returns>
		/// True if the value equals the current fraction, false otherwise (including for
		/// non-Fraction types or null object.
		/// </returns>
		public override bool Equals(object obj) {
			if (obj == null || !(obj is Fraction)) {
				return false;
			}

			try {
				Fraction right = (Fraction)obj;
				return this.compareEquality(right, false);
			} catch {
				// can't throw in an Equals!
				return false;
			}
		}

		/// <summary>
		/// Returns a hash code generated from the current Fraction.
		/// </summary>
		/// <returns>The hash code.</returns>
		/// <remarks>
		/// Reduces (in-place) the Fraction first.
		/// </remarks>
		public override int GetHashCode() {
			// insure we're as close to normalized as possible first
			ReduceFraction();
			int numeratorHash = this.numerator.GetHashCode();
			int denominatorHash = this.denominator.GetHashCode();
			return (numeratorHash ^ denominatorHash);
		}
		#endregion

		#region IComparable member and type-specific version
		/// <summary>
		/// Compares an object to this Fraction.
		/// </summary>
		/// <param name="obj">The object to compare against (null is less than everything).</param>
		/// <returns>
		/// -1 if this is less than,
		///  0 if they are equal,
		///  1 if this is greater than
		/// </returns>
		/// <remarks>
		/// Will convert an object from longs, doubles, and strings as this is a value-type.
		/// </remarks>
		public int CompareTo(object obj) {
			if (obj == null) {
				return 1;
			}
			// null is less than anything
			Fraction right;
			if (obj is Fraction) {
				right = (Fraction)obj;
			} else if (obj is long) {
				right = (long)obj;
			} else if (obj is double) {
				right = (double)obj;
			} else if (obj is string) {
				right = (string)obj;
			} else {
				throw new ArgumentException("Must be convertible to Fraction", "obj");
			}
			return this.CompareTo(right);
		}

		/// <summary>
		/// Compares this Fraction to another Fraction.
		/// </summary>
		/// <param name="right">The Fraction to compare against.</param>
		/// <returns>
		/// -1 if this is less than,
		///  0 if they are equal,
		///  1 if this is greater than.
		/// </returns>
		public int CompareTo(Fraction right) {
			// if left is an indeterminate, punt to the helper...
			if (this.denominator == 0) {
				return indeterminantCompare(NormalizeIndeterminate(this.numerator), right);
			}
			// if right is an indeterminate, punt to the helper...
			if (right.denominator == 0) {
				// note sign-flip...
				return -indeterminantCompare(NormalizeIndeterminate(right.numerator), this);
			}
			// they're both normal Fractions
			CrossReducePair(this, right);
			try {
				checked {
					long leftScale = this.numerator * right.denominator;
					long rightScale = this.denominator * right.numerator;
					if (leftScale < rightScale)
						return -1;
					else if (leftScale > rightScale)
						return 1;
					else
						return 0;
				}
			} catch (Exception e) {
				throw new FractionException(string.Format("CompareTo({0}, {1}) error", this, right), e);
			}
		}
		#endregion

		#region IFormattable Members
		string System.IFormattable.ToString(string format, IFormatProvider formatProvider) {
			return this.numerator.ToString(format, formatProvider) + "/" + this.denominator.ToString(format, formatProvider);
		}
		#endregion

		#region Reduction
		/// <summary>
		/// Reduces (simplifies) a Fraction by dividing down to lowest possible denominator (via GCD).
		/// </summary>
		/// <param name="frac">The Fraction to be reduced [WILL BE MODIFIED IN PLACE].</param>
		/// <remarks>
		/// Modifies the input arguments in-place! Will normalize the NaN and infinites
		/// representation. Will set Denominator to 1 for any zero numerator. Moves sign to the
		/// Numerator. 2/4 will be reduced to 1/2.
		/// </remarks>
		public void ReduceFraction() {
			// clean up the NaNs and infinites
			if (this.denominator == 0) {
				this.numerator = (long)NormalizeIndeterminate(this.numerator);
				return;
			}
			// all forms of zero are alike.
			if (this.numerator == 0) {
				this.denominator = 1;
				return;
			}
			long iGCD = GCD(this.numerator, this.denominator);
			this.numerator /= iGCD;
			this.denominator /= iGCD;
			// if negative sign in denominator
			if (this.denominator < 0) {
				//move negative sign to numerator
				this.numerator = -this.numerator;
				this.denominator = -this.denominator;
			}
		}

		/// <summary>
		/// Reduces (simplifies) a Fraction by dividing down to lowest possible denominator (via GCD).
		/// </summary>
		/// <param name="frac">The Fraction to be reduced [WILL BE MODIFIED IN PLACE].</param>
		/// <remarks>
		/// Modifies the input arguments in-place! Will normalize the NaN and infinites
		/// representation. Will set Denominator to 1 for any zero numerator. Moves sign to the
		/// Numerator. 2/4 will be reduced to 1/2.
		/// </remarks>
		public void ReduceFraction(Fraction fraction) {
			// clean up the NaNs and infinites
			if (fraction.denominator == 0) {
				fraction.numerator = (long)NormalizeIndeterminate(fraction.numerator);
				return;
			}
			// all forms of zero are alike.
			if (fraction.numerator == 0) {
				fraction.denominator = 1;
				return;
			}
			long iGCD = GCD(fraction.numerator, fraction.denominator);
			fraction.numerator /= iGCD;
			fraction.denominator /= iGCD;
			// if negative sign in denominator
			if (fraction.denominator < 0) {
				//move negative sign to numerator
				fraction.numerator = -fraction.numerator;
				fraction.denominator = -fraction.denominator;
			}
		}

		/// <summary>
		/// Cross-reduces a pair of Fractions so that we have the best GCD-reduced values for multiplication.
		/// </summary>
		/// <param name="frac1">The first Fraction [WILL BE MODIFIED IN PLACE].</param>
		/// <param name="frac2">The second Fraction [WILL BE MODIFIED IN PLACE].</param>
		/// <remarks>
		/// Modifies the input arguments in-place! (3/4, 5/9) = (1/4, 5/3).
		/// </remarks>
		public static void CrossReducePair(Fraction frac1, Fraction frac2) {
			// leave the indeterminates alone!
			if (frac1.denominator == 0 || frac2.denominator == 0) {
				return;
			}
			long gcdTop = GCD(frac1.numerator, frac2.denominator);
			frac1.numerator = frac1.numerator / gcdTop;
			frac2.denominator = frac2.denominator / gcdTop;

			long gcdBottom = GCD(frac1.denominator, frac2.numerator);
			frac2.numerator = frac2.numerator / gcdBottom;
			frac1.denominator = frac1.denominator / gcdBottom;
		}
		#endregion

		#region Implementation
		#region Convert a double to a fraction
		private static Fraction convertPositiveDouble(int sign, double inValue) {
			// Shamelessly stolen from http://homepage.smc.edu/kennedy_john/CONFRAC.PDF
			// with AccuracyFactor == double.Episilon
			long fractionNumerator = (long)inValue;
			double fractionDenominator = 1;
			double previousDenominator = 0;
			double remainingDigits = inValue;
			int maxIterations = 594;
			// found at http://www.ozgrid.com/forum/archive/index.php/t-22530.html
			while (remainingDigits != Math.Floor(remainingDigits)
				&& Math.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon) {
				remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));
				double scratch = fractionDenominator;
				fractionDenominator = (Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
				fractionNumerator = (long)(inValue * fractionDenominator + 0.5);
				previousDenominator = scratch;
				if (maxIterations-- < 0) {
					break;
				}
			}
			return new Fraction(fractionNumerator * sign, (long)fractionDenominator);
		}
		#endregion

		#region Equality helper
		/// <summary>
		/// Compares for equality the current Fraction to the value passed.
		/// </summary>
		/// <param name="right">A Fraction to compare against.</param>
		/// <param name="notEqualCheck">If true, we're looking for not-equal.</param>
		/// <returns>
		/// True if the  equals the current fraction, false otherwise.
		/// If comparing two NaNs, they are always equal AND not-equal.
		/// </returns>
		private bool compareEquality(Fraction right, bool notEqualCheck) {
			// insure we're normalized first
			ReduceFraction();
			// now normalize the comperand
			ReduceFraction(right);
			if (this.numerator == right.numerator && this.denominator == right.denominator) {
				// special-case rule, two NaNs are always both equal
				if (notEqualCheck && this.IsNaN()) {
					return true;
				} else {
					return !notEqualCheck;
				}
			} else {
				return notEqualCheck;
			}
		}
		#endregion

		#region Comparison helper
		/// <summary>
		/// Determines how this Fraction, of an indeterminate type, compares to another Fraction.
		/// </summary>
		/// <param name="leftType">What kind of indeterminate.</param>
		/// <param name="right">The other Fraction to compare against.</param>
		/// <returns>
		/// -1 if this is less than,
		///  0 if they are equal,
		///  1 if this is greater than.
		/// </returns>
		/// <remarks>
		/// NaN is less than anything except NaN and Negative Infinity. Negative Infinity is less
		/// than anything except Negative Infinity. Positive Infinity is greater than anything except
		/// Positive Infinity.
		/// </remarks>
		private static int indeterminantCompare(Indeterminates leftType, Fraction right) {
			switch (leftType) {
				case Indeterminates.NaN:
					// A NaN is...
					if (right.IsNaN()) {
						return 0; // equal to a NaN
					} else if (right.IsNegativeInfinity()) {
						return 1; // great than Negative Infinity
					} else {
						return -1; // less than anything else
					}
				case Indeterminates.NegativeInfinity:
					// Negative Infinity is...
					if (right.IsNegativeInfinity()) {
						return 0;	// equal to Negative Infinity
					} else {
						return -1;	// less than anything else
					}
				case Indeterminates.PositiveInfinity:
					if (right.IsPositiveInfinity()) {
						return 0;	// equal to Positive Infinity
					} else {
						return 1;	// greater than anything else
					}
				default:
					// this CAN'T happen, something VERY wrong is going on...
					return 0;
			}
		}
		#endregion

		#region Math helpers
		/// <summary>
		/// Negates the Fraction.
		/// </summary>
		/// <param name="frac">Value to negate.</param>
		/// <returns>A new Fraction that is sign-flipped from the input.</returns>
		private static Fraction negate(Fraction frac) {
			// for a NaN, it's still a NaN
			return new Fraction(-frac.numerator, frac.denominator);
		}

		/// <summary>
		/// Adds two Fractions.
		/// </summary>
		/// <param name="left">A Fraction.</param>
		/// <param name="right">Another Fraction.</param>
		/// <returns>Sum of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
		/// <remarks>
		/// Will throw if an overflow occurs when computing the GCD-normalized values.
		/// </remarks>
		private static Fraction add(Fraction left, Fraction right) {
			if (left.IsNaN() || right.IsNaN())
				return NaN;
			long gcd = GCD(left.denominator, right.denominator);
			// cannot return less than 1
			long leftDenominator = left.denominator / gcd;
			long rightDenominator = right.denominator / gcd;
			try {
				checked {
					long numerator = left.numerator * rightDenominator + right.numerator * leftDenominator;
					long denominator = leftDenominator * rightDenominator * gcd;
					return new Fraction(numerator, denominator);
				}
			} catch (Exception e) {
				throw new FractionException("Add error", e);
			}
		}

		/// <summary>
		/// Multiplies two Fractions.
		/// </summary>
		/// <param name="left">A Fraction.</param>
		/// <param name="right">Another Fraction.</param>
		/// <returns>Product of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
		/// <remarks>
		/// Will throw if an overflow occurs. Does a cross-reduce to
		/// insure only the unavoidable overflows occur.
		/// </remarks>
		private static Fraction multiply(Fraction left, Fraction right) {
			if (left.IsNaN() || right.IsNaN()) {
				return NaN;
			}
			// this would be unsafe if we were not a ValueType, because we would be changing the
			// caller's values.  If we change back to a class, must use temporaries
			CrossReducePair(left, right);
			try {
				checked {
					long numerator = left.numerator * right.numerator;
					long denominator = left.denominator * right.denominator;
					return new Fraction(numerator, denominator);
				}
			} catch (Exception e) {
				throw new FractionException("Multiply error", e);
			}
		}

		/// <summary>
		/// Returns the modulus (remainder after dividing) two Fractions.
		/// </summary>
		/// <param name="left">A Fraction.</param>
		/// <param name="right">Another Fraction</param>
		/// <returns>modulus of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
		/// <remarks>
		/// Will throw if an overflow occurs. Does a cross-reduce to
		/// insure only the unavoidable overflows occur.
		/// </remarks>
		private static Fraction modulus(Fraction left, Fraction right) {
			if (left.IsNaN() || right.IsNaN()) {
				return NaN;
			}
			try {
				checked {
					// this will discard any fractional places...
					Int64 quotient = (Int64)(left / right);
					Fraction whole = new Fraction(quotient * right.numerator, right.denominator);
					return left - whole;
				}
			} catch (Exception e) {
				throw new FractionException("Modulus error", e);
			}
		}

		/// <summary>
		/// Computes the greatest common divisor for two values.
		/// </summary>
		/// <param name="left">One value.</param>
		/// <param name="right">Another value.</param>
		/// <returns>
		/// The greatest common divisor of the two values
		/// (6, 9) returns 3 and (11, 4) returns 1
		/// </returns>
		private static long GCD(long left, long right) {
			// take absolute values
			if (left < 0) {
				left = -left;
			}
			if (right < 0) {
				right = -right;
			}
			// if we're dealing with any zero or one, the GCD is 1
			if (left < 2 || right < 2) {
				return 1;
			}

			do {
				if (left < right) {
					long temp = left;
					// swap the two operands
					left = right;
					right = temp;
				}
				left %= right;
			} while (left != 0);

			return right;
		}
		#endregion

		#region Indeterminate helpers
		/// <summary>
		/// Gives the culture-related representation of the indeterminate types NaN, PositiveInfinity
		/// and NegativeInfinity.
		/// </summary>
		/// <param name="numerator">The value in the numerator.</param>
		/// <returns>The culture-specific string representation of the implied value.</returns>
		/// <remarks>
		/// Only the sign and zero/non-zero information is relevant.
		/// </remarks>
		private static string indeterminateTypeName(long numerator) {
			// could also be NumberFormatInfo.InvariantInfo
			NumberFormatInfo info = NumberFormatInfo.CurrentInfo;
			switch (NormalizeIndeterminate(numerator)) {
				case Indeterminates.PositiveInfinity:
					return info.PositiveInfinitySymbol;
				case Indeterminates.NegativeInfinity:
					return info.NegativeInfinitySymbol;
				default:
				// if this happens, something VERY wrong is going on...
				case Indeterminates.NaN:
					return info.NaNSymbol;
			}
		}

		/// <summary>
		/// Gives the normalize representation of the indeterminate types NaN, PositiveInfinity
		/// and NegativeInfinity.
		/// </summary>
		/// <param name="numerator">The value in the numerator.</param>
		/// <returns>The normalized version of the indeterminate type.</returns>
		/// <remarks>
		/// Only the sign and zero/non-zero information is relevant.
		/// </remarks>
		private static Indeterminates NormalizeIndeterminate(long numerator) {
			switch (Math.Sign(numerator)) {
				case 1:
					return Indeterminates.PositiveInfinity;
				case -1:
					return Indeterminates.NegativeInfinity;
				default:
				// if this happens, your Math.Sign function is BROKEN!
				case 0:
					return Indeterminates.NaN;
			}
		}

		/// <summary>
		/// These are used to represent the indeterminate with a Denominator of zero.
		/// </summary>
		private enum Indeterminates {
			/// <summary>
			/// Zero denominator fraction.
			/// </summary>
			NaN = 0,

			/// <summary>
			/// Positive infinity fraction.
			/// </summary>
			PositiveInfinity = 1,

			/// <summary>
			/// Negative infinity fraction.
			/// </summary>
			NegativeInfinity = -1
		}
		#endregion


		#endregion
	}

	/// <summary>
	/// Exception class for Fraction, derived from System.Exception.
	/// </summary>
	public class FractionException : Exception {
		/// <summary>
		/// Constructs a FractionException.
		/// </summary>
		/// <param name="Message">String associated with the error message.</param>
		/// <param name="InnerException">Actual inner exception caught.</param>
		public FractionException(string Message, Exception InnerException)
			: base(Message, InnerException) {
		}
	}
}
