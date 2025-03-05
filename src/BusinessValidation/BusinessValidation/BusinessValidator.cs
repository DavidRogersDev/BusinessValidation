using System;

namespace BusinessValidation
{

    /// <summary>
    /// Base class for business validators.
    /// </summary>
    /// <typeparam name="T">The type of the validationObject which will provide all the information required to run the validation logic.</typeparam>
    public abstract class BusinessValidator<T> : IBusinessValidator<T>
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected BusinessValidator()
        {
            Validator = new Validator();
        }

        /// <summary>
        /// Constructor which takes a <see cref="BusinessValidation.Validator"/> as a parameter.
        /// </summary>
        /// <param name="validator">A <see cref="BusinessValidation.Validator" /> which will be used to assist in performing validation.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validator"/> is <c>null</c>.</exception>
        protected BusinessValidator(Validator validator)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// A <see cref="BusinessValidation.Validator"/> which can be used in the Validate method.
        /// </summary>
        public virtual Validator Validator { get; }

        /// <summary>
        /// Base method for the Validation operation. This must be implemented by derived classes.
        /// </summary>
        /// <returns>A <see cref="BusinessValidationResult"/> representing the result of the validation check.</returns>
        public abstract BusinessValidationResult Validate(T validationObject);
    }
}
