using BusinessValidation;
using System;
using System.Linq;

namespace Winforms.Sample.Ui.Validation
{
    public class UnitIdValidator : BusinessValidator<UnitValidationObject>
    {
        public override BusinessValidationResult Validate(UnitValidationObject validationObject)
        {
            Validator.Validate(v => v.Id, "{fail-bundle} must be non-zero.", validationObject, v => v.Id > 0);

            return new BusinessValidationResult(Validator);
        }
    }
}
