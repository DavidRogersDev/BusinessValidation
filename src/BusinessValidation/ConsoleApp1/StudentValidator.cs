using System;
using BusinessValidation;
using System.Linq;

namespace ConsoleApp1
{
    public class StudentValidator : BusinessValidator
    {
        public StudentValidator()
            : base()
        {
        }        

        public override void ExecuteValidation()
        {
            Validator.AddFailure("Bad", "Bad thing happened");
            Validator.Validate("Bad", "Really Bad thing happened", false);
        }
    }
}
