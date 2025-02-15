using System;
using BusinessValidation;
using System.Linq;

namespace ConsoleApp1
{
    public class OtherStudentValidator : BusinessValidator<Student>, IBusinessValidator<Student>
    {
        public OtherStudentValidator()
            : base()
        {
        }


        public override BusinessValidationResult Validate(Student validationObject)
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);
            Validator.Validate("Other", "Really Bad thing happened", false);

            return new BusinessValidationResult(Validator.ValidationFailures);
        }
    }
}
