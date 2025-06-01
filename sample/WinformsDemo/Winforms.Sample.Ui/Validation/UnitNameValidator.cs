using BusinessValidation;
using System;
using System.Linq;

namespace Winforms.Sample.Ui.Validation
{
    public class UnitNameValidator : BusinessValidator<UnitValidationObject>
    {
        private const string ReservedName = "Alchemy 101";

        public override BusinessValidationResult Validate(UnitValidationObject validationObject)
        {
            Validator.Validate(v => v.Name, $"{ReservedName} is a reserved {{fail-bundle}} and cannot be used.", validationObject, v => !v.Name.Equals(ReservedName, StringComparison.InvariantCultureIgnoreCase));
            Validator.Validate(v => v.Name, "{fail-bundle} cannot be longer than 10 characters.", validationObject, v => v.Name.Length < 11);

            IReadOnlyDictionary<string, IReadOnlyList<string>> validationFailures = Validator.ValidationFailures;

            return new BusinessValidationResult(validationFailures);
        }
    }
}
