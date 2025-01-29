using BusinessValidation;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public class SubjectValidator : BusinessValidator
    {
        public SubjectValidator()
            : base()
        {
        }

        public override void ExecuteValidation()
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);
        }
    }
}
