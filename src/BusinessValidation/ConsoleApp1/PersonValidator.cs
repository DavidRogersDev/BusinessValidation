using BusinessValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PersonValidator : BusinessValidator<Student>, IBusinessValidator<Student>
    {
        public PersonValidator()
        {
        }


        public override MyBusinessValidationResult Validate(Student validationObject)
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);
            Validator.Validate("Other", "Really Bad thing happened", false);
            Validator.Validate("MyTest", "My Really Bad thing happened", false);
            Validator.Validate("MyTest", "My Really Really Bad thing happened", false);

            return new MyBusinessValidationResult(Validator.ValidationFailures);
        }
    }

    public class MyBusinessValidationResult : BusinessValidationResult
    {
        public MyBusinessValidationResult(IReadOnlyDictionary<string, IReadOnlyList<string>> validationFailures)
            : base(validationFailures)
        {
        }

        public override bool IsValid => base.IsValid;
        public override bool NotValid => ValidationFailures.Any();
        IReadOnlyDictionary<string, IReadOnlyList<string>> validationFailures;
        public override IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures
        {
            get => validationFailures;
            set => validationFailures = value;
        }
    }
}
