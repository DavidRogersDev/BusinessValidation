using System;
using System.Collections.Generic;

namespace BusinessValidation
{
    public sealed class ValidationFailureException : Exception
    {
        public readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Failures;

        public ValidationFailureException(IReadOnlyDictionary<string, IReadOnlyList<string>> failures)
        {
            Failures = failures;
        }

        public ValidationFailureException(string message, IReadOnlyDictionary<string, IReadOnlyList<string>> failures)
            : base(message)
        {
            Failures = failures;
        }
    }
}
