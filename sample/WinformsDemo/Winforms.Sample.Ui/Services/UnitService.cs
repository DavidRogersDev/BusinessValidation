using BusinessValidation;
using Winforms.Sample.Ui.Validation;
using Winforms.Sample.Domain;

namespace Winforms.Sample.Ui.Services
{
    public class UnitService : IUnitService
    {
        readonly IBusinessValidator<UnitValidationObject> unitNameValidator;
        readonly IBusinessValidator<UnitValidationObject> unitIdValidator;
        public UnitService(IEnumerable<IBusinessValidator<UnitValidationObject>> unitValidators)
        {
            this.unitIdValidator = unitValidators.Where(v => v.GetType().Equals(typeof(UnitIdValidator))).Single();
            this.unitNameValidator = unitValidators.Except([unitIdValidator]).Single();
        }

        public bool CreateUnit(Unit unit)
        {
            UnitValidationObject unitValidationObject = new() { Id = unit.Id, Name = unit.Name };

            BusinessValidationResult idResult = unitIdValidator.Validate(unitValidationObject);
            BusinessValidationResult nameResult = unitNameValidator.Validate(unitValidationObject);

            BusinessValidationResult merged = idResult.Merge(nameResult);

            if (merged.NotValid) throw new ValidationFailureException(merged.ValidationFailures);


            return true;
        }
    }
}
