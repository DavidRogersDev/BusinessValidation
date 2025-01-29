using BusinessValidation;
using System;
using System.Linq;

namespace ValidationLibrary
{
    public interface IBusValidator<T> 
        where T : IBusVal
    {
        void ExecuteValidation();
        Validator Validator { get; }
    }

    public class BusValidator<T> : IBusValidator<T>
        where T : IBusVal
    {
        readonly T busValidator;
        public BusValidator(T validator)
        {
            this.busValidator = validator;
        }

        public Validator Validator => busValidator.Validator;

        public void ExecuteValidation()
        {
            busValidator.ExecuteValidation();
        }
    }
}
