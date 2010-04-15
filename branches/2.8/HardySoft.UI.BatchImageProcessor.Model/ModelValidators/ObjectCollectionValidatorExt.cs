using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections;

namespace HardySoft.UI.BatchImageProcessor.Model.ModelValidators {
	/// <summary>
	/// Due to the issue I described in http://entlib.codeplex.com/Thread/View.aspx?ThreadId=82809
	/// startting from 4th post, I created my own validation here.
	/// </summary>
	public class ObjectCollectionValidatorExt : Validator {
		private Type targetType;
		private string targetRuleset;
		//private Validator targetTypeValidator;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorExt"/> for a target type.</para>
		/// </summary>
		/// <param name="targetType">The target type</param>
		/// <remarks>
		/// The default ruleset for <paramref name="targetType"/> will be used.
		/// </remarks>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		public ObjectCollectionValidatorExt(Type targetType)
			: this(targetType, string.Empty) {
		}

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorExt"/> for a target type
		/// using the supplied ruleset.</para>
		/// </summary>
		/// <param name="targetType">The target type</param>
		/// <param name="targetRuleset">The ruleset to use.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="targetType"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">when <paramref name="targetRuleset"/> is <see langword="null"/>.</exception>
		public ObjectCollectionValidatorExt(Type targetType, string targetRuleset)
			: base(null, null) {
			if (targetType == null) {
				throw new ArgumentNullException("targetType");
			}
			if (targetRuleset == null) {
				throw new ArgumentNullException("targetRuleset");
			}

			this.targetType = targetType;
			this.targetRuleset = targetRuleset;
			//this.targetTypeValidator = ValidationFactory.CreateValidator(this.targetType, this.targetRuleset);
		}

		/// <summary>
		/// Validates by applying the validation rules for the target type specified for the receiver to the elements
		/// in <paramref name="objectToValidate"/>.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
		/// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
		/// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
		/// <remarks>
		/// If <paramref name="objectToValidate"/> is <see langword="null"/> validation is ignored.
		/// <para/>
		/// A referece to a non collection object causes a validation failure and the validation rules
		/// for the configured target type will not be applied.
		/// <para/>
		/// Elements in the collection of a type not compatible with the configured target type causes a validation failure but
		/// do not affect validation on other elements.
		/// </remarks>
		protected override void DoValidate(object objectToValidate,
			object currentTarget,
			string key,
			ValidationResults validationResults) {
			if (objectToValidate != null) {
				IEnumerable enumerable = objectToValidate as IEnumerable;
				if (enumerable != null) {
					foreach (object element in enumerable) {
						if (element != null) {
							if (this.targetType.IsAssignableFrom(element.GetType())) {
								// reset the current target and the key
								//this.targetTypeValidator.DoValidate(element, element, null, validationResults);
								Validator validator = ValidationFactory.CreateValidator(element.GetType());
								validator.Validate(element, validationResults);
							} /*else {
								// unlikely
								this.LogValidationResult(validationResults,
									"The element in the validated collection is not compatible with the expected type.",
									element,
									null);
							}*/
						}
					}
				} else {
					this.LogValidationResult(validationResults, "The supplied object is not a collection.", currentTarget, key);
				}
			}
		}

		/// <summary>
		/// Gets the message template to use when logging results no message is supplied.
		/// </summary>
		protected override string DefaultMessageTemplate {
			get {
				return null;
			}
		}

		#region test only properties

		internal Type TargetType {
			get {
				return this.targetType;
			}
		}

		internal string TargetRuleset {
			get {
				return this.targetRuleset;
			}
		}

		#endregion
	}
}
