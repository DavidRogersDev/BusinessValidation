using BusinessValidation;

namespace ValidationLibrary
{
    public class ValidationRule : IBusVal
    {

        public ValidationRule()
        {
            Validator = new Validator();
        }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures => Validator.ValidationFailures;

        public Validator Validator { get; init; }

        public void ExecuteValidation()
        {
            Validator.Validate("bad", "was bad", false);
        }
    }



    public interface IBusVal
    {
        Validator Validator { get; }
        void ExecuteValidation();
        IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures { get; }
    }
}
