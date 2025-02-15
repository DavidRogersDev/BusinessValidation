using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessValidation
{
    public class BusinessValidationResult
    {
        public BusinessValidationResult(IReadOnlyDictionary<string, IReadOnlyList<string>> validationFailures)
        {
            ValidationFailures = validationFailures;
        }
        
        public BusinessValidationResult(Validator validator)
        {
            ValidationFailures = validator.ValidationFailures;
        }

        public bool IsValid => !ValidationFailures.Any();
        public bool NotValid => ValidationFailures.Any();
        public IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures { get; set; }
    }
}
