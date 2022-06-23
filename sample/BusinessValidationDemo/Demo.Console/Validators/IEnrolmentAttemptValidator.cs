

using BusinessValidation;

namespace Demo.Console.Validators
{
    public interface IEnrolmentAttemptValidator : IValidator
    {
        Validator IsEnrolled(string unitCode, string studentId);
        Validator IsEligible(string unitCode, string studentId);
        Validator IsFinancial(string unitCode);
    }
}
