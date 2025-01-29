using System;

namespace BusinessValidation
{
    public interface IBusinessValidator<T>
        where T : BusinessValidator
    {
        void ExecuteValidation();
        Validator Validator { get; }
    }

    public abstract class BusinessValidator
    {
        public BusinessValidator()
        {
            Validator = new Validator();
        }

        public Validator Validator { get; }

        public abstract void ExecuteValidation();
    }
}
