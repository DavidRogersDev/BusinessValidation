using BusinessValidation;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public class SubjectValidator : BusinessValidator<Subject>, IBusinessValidator<Subject>
    {
        public SubjectValidator()
            : base()
        {
        }

        public override BusinessValidationResult Validate(Subject validationObject)
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);
            Validator.Validate("Sunbject", "Really Bad thing happened in a sub", false);

            return new BusinessValidationResult(Validator.ValidationFailures);
        }
    }
}
