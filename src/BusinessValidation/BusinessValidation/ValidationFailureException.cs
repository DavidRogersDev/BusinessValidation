using System;
using System.Collections.Generic;

namespace BusinessValidation
{
    /// <summary>
    /// A class which represents validation failures where some validation logic has failed.
    /// </summary>
    public sealed class ValidationFailureException : Exception
    {
        /// <summary>
        /// A <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" /> containing validation failures, grouped by a string key.
        /// </summary>
        public readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Failures;

        /// <summary>
        /// Constructor taking an <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" /> of validation failures.
        /// </summary>
        /// <param name="failures">Type: <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" />. A grouped collection of validation failures.</param>
        public ValidationFailureException(IReadOnlyDictionary<string, IReadOnlyList<string>> failures)
        {
            Failures = failures;
        }

        /// <summary>
        /// Constructor taking a message and an <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" /> of validation failures.
        /// </summary>
        /// <param name="message">Type: <see cref="string"/>. A message for the exception.</param>
        /// <param name="failures">Type: <see cref="IReadOnlyDictionary&lt;string, IReadOnlyList&lt;string&lt;&gt;" />. A grouped collection of validation failures.</param>
        public ValidationFailureException(string message, IReadOnlyDictionary<string, IReadOnlyList<string>> failures)
            : base(message)
        {
            Failures = failures;
        }
    }
}
