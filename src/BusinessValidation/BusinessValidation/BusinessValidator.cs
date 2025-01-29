using System;

namespace BusinessValidation
{
    public class BusinessValidator<T> : IBusinessValidator<T>
        where T : BusinessValidator
    {
        private readonly T businessValidator;
        public BusinessValidator(T validator)
        {
            businessValidator = validator;
        }

        public Validator Validator => this.businessValidator.Validator;

        public void ExecuteValidation()
        {
            this.businessValidator.ExecuteValidation();
        }
    }
}
