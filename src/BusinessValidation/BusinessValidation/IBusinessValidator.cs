using System;

namespace BusinessValidation
{
    public interface IBusinessValidator<in T>
    {
        BusinessValidationResult Validate(T validationObject);
        Validator Validator { get; }
    }
}
