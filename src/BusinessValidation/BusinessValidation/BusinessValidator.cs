using System;

namespace BusinessValidation
{
    public abstract class BusinessValidator<T> : IBusinessValidator<T>
    {
        public BusinessValidator()
        {
            Validator = new Validator();
        }
        
        public BusinessValidator(Validator validator)
        {
            Validator = validator;
        }

        public virtual Validator Validator { get; }

        public abstract BusinessValidationResult Validate(T validationObject);
    }
}
