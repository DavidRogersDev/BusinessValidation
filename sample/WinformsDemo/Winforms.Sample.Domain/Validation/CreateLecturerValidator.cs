using BusinessValidation;
using System;
using System.Linq;

namespace Winforms.Sample.Domain.Validation
{
    public class CreateLecturerValidator : BusinessValidator<CreateLecturerValidationObject>
    {
        private const string ReservedName = "Beelzebub";

        public override BusinessValidationResult Validate(CreateLecturerValidationObject validationObject)
        {
            Validator.Validate(l => l.Name, "Name must be less than 10 characters.", validationObject, l => l.Name.Length <= 8);
            Validator.Validate(l => l.Name, $"{ReservedName} is a reserved {{fail-bundle}} and cannot be used.", validationObject, l => !l.Name.Equals(ReservedName, StringComparison.InvariantCultureIgnoreCase));

            return new BusinessValidationResult(Validator);
        }
    }
}
