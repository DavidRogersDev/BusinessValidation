using System;
using System.Linq;

namespace BusinessValidation.Tests.IntegrationTestClasses
{
    public class StudentValidator : BusinessValidator<Student>
    {
        public override BusinessValidationResult Validate(Student validationObject)
        {
            return new BusinessValidationResult(Validator);
        }
    }
}
