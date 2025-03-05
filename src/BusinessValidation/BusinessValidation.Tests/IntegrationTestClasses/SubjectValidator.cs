using System;
using System.Linq;

namespace BusinessValidation.Tests.IntegrationTestClasses
{
    public class SubjectValidator : BusinessValidator<Subject>
    {
        public override BusinessValidationResult Validate(Subject validationObject)
        {
            Validator.Validate(s => s.Name, "{fail-bundle} must be less than 100 characters long.", validationObject, s => s.Name.Length < 100);

            return new BusinessValidationResult(Validator);
        }
    }
}
