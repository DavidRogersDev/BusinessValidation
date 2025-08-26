using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessValidation
{
    /// <summary>
    /// The result of a business validation operation.
    /// </summary>
    public class BusinessValidationResult
    {
        /// <summary>
        /// Constructor which takes a dictionary of validation failures.
        /// </summary>
        /// <param name="validationFailures"><see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" /> of validation failures.</param>
        public BusinessValidationResult(IReadOnlyDictionary<string, IReadOnlyList<string>> validationFailures)
        {
            ValidationFailures = validationFailures;
        }

        /// <summary>
        /// Constructor which takes a <see cref="Validator"/>.
        /// </summary>
        /// <param name="validator">A <see cref="Validator"/> which contains the result of a validation operation.</param>
        public BusinessValidationResult(Validator validator)
        {
            ValidationFailures = validator.ValidationFailures;
        }

        /// <summary>
        /// A <see cref="Boolean"/> indicating whether a validation operation has been successful or not. In this case, whether valid.
        /// </summary>
        public virtual bool IsValid => !ValidationFailures.Any();

        /// <summary>
        /// A <see cref="Boolean"/> indicating whether a validation operation has been successful or not. In this case, whether invalid.
        /// </summary>
        public virtual bool NotValid => ValidationFailures.Any();

        /// <summary>
        /// The validation failures <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" /> which is the core of the result.
        /// </summary>
        public virtual IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures { get; set; }

        /// <summary>
        /// Merges one <see cref="BusinessValidationResult"/> object with another.
        /// </summary>
        /// <param name="other">Another <see cref="BusinessValidationResult"/> object to merge with the one invoking this method.</param>
        /// <returns>The <see cref="BusinessValidationResult"/> object to enable chaining.</returns>
        public virtual BusinessValidationResult Merge(BusinessValidationResult other)
        {
            if (other is null || other.IsValid)
                return this;

            if (IsValid && other.NotValid)
                return other;

            // if reach here, both this and other are invalid.
            var newValidator = new Validator();

            foreach (var failBundle in ValidationFailures)
            {
                foreach (var failureMessage in failBundle.Value)
                    newValidator.AddFailure(failBundle.Key, failureMessage);
            }

            foreach (var errorKey in other.ValidationFailures)
            {
                foreach (var failureMessage in errorKey.Value)
                    newValidator.AddFailure(errorKey.Key, failureMessage);
            }

            return new BusinessValidationResult(newValidator);
        }

        public static implicit operator bool(BusinessValidationResult businessValidationResult)
        {
            if (businessValidationResult is null)
            {
                throw new ArgumentNullException(nameof(businessValidationResult));
            }

            return businessValidationResult.IsValid;
        }

        public static bool operator true(BusinessValidationResult businessValidationResult)
        {
            return businessValidationResult;
        }

        public static bool operator false(BusinessValidationResult businessValidationResult)
        {
            return !businessValidationResult;
        }

        /// <summary>
        /// String indexer. Fail-bundle key.
        /// </summary>
        /// <param name="index">Index of item to retrieve.</param>
        /// <returns>List of failure messages for the fail-bundle.</returns>
        public IReadOnlyList<string> this[string index]
        {
            get
            {
                return ValidationFailures[index];
            }
        }
    }
}
