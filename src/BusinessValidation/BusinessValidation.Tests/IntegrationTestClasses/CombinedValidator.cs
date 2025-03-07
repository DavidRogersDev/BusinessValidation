namespace BusinessValidation.Tests.IntegrationTestClasses
{
    public class CombinedValidator : IBusinessValidator<Student>, IBusinessValidator<Subject>
    {
        public Validator Validator { get; }

        public CombinedValidator()
        {
            Validator = new Validator();
        }

        public BusinessValidationResult Validate(Subject validationObject)
        {
            Validator.Validate(s => s.Name, "{fail-bundle} must be less than 100 characters long.", validationObject, s => s.Name.Length < 100);

            return new BusinessValidationResult(Validator);

        }

        public BusinessValidationResult Validate(Student validationObject)
        {
            Validator.Validate(s => s.Id, "{fail-bundle} must have a positive value.", validationObject, s => s.Id > 0);

            return new BusinessValidationResult(Validator);
        }
    }
}
