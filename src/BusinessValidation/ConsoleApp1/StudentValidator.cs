using System;
using BusinessValidation;
using System.Linq;

namespace ConsoleApp1
{
    public class StudentValidator : BusinessValidator<Student>, IBusinessValidator<Student>
    {
        public override BusinessValidationResult Validate(Student validationObject)
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);

            return new BusinessValidationResult(Validator.ValidationFailures);
        }
    }
}
