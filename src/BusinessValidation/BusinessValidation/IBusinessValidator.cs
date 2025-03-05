using System;

namespace BusinessValidation
{
    /// <summary>
    /// Contract for a validator to perform business validation.
    /// </summary>
    /// <typeparam name="T">The type of the validationObject which will provide all the information required to run the validation logic.</typeparam>
    public interface IBusinessValidator<in T>
    {
        /// <summary>
        /// Method for the Validation operation.
        /// </summary>
        /// <param name="validationObject">The object which will provide all the information required to run the validation logic.</param>
        /// <returns>A <see cref="BusinessValidationResult"/> representing the result of the validation check.</returns>
        BusinessValidationResult Validate(T validationObject);

        /// <summary>
        /// A <see cref="BusinessValidation.Validator"/> which can be used in the Validate method. This is also exposed if consumers want to work with it directly following validation.
        /// </summary>
        Validator Validator { get; }
    }
}
