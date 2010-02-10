using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace HardySoft.UI.BatchImageProcessor.Model.ModelValidators {
	/// <summary>
	/// Represents an <see cref="ObjectValidator"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Parameter
		| AttributeTargets.Class,
		AllowMultiple = true,
		Inherited = false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
		Justification = "Fields are used internally")]
	public class ObjectCollectionValidatorExtAttribute : ValidatorAttribute {
		private Type targetType;
		private string targetRuleset;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class with a target type.</para>
		/// </summary>
		/// <param name="targetType">The target type.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		public ObjectCollectionValidatorExtAttribute(Type targetType)
			: this(targetType, string.Empty) {
		}

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorAttribute"/> class with a target type
		/// and a specific ruleset.</para>
		/// </summary>
		/// <param name="targetType">The target type.</param>
		/// <param name="targetRuleset">The target ruleset.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
		/// <seealso cref="ObjectValidator(Type, string)"/>
		public ObjectCollectionValidatorExtAttribute(Type targetType, string targetRuleset) {
			if (targetType == null) {
				throw new ArgumentNullException("targetType");
			}
			if (targetRuleset == null) {
				throw new ArgumentNullException("targetRuleset");
			}

			this.targetType = targetType;
			this.targetRuleset = targetRuleset;
		}

		/// <summary>
		/// Creates the <see cref="ObjectValidator"/> described by attribute.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="ObjectValidator"/>.</returns>
		/// <remarks>The supplied <paramref name="targetType"/> is the type of the element to which the
		/// attribute is bound, not the element collection type that has been set in the constructor.</remarks>
		protected override Validator DoCreateValidator(Type targetType) {
			// avoid supplied target type - that's the type of the collection
			return new ObjectCollectionValidatorExt(this.targetType, this.targetRuleset);
		}
	}
}
