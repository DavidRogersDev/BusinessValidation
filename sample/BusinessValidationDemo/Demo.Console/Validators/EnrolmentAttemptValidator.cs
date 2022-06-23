using BusinessValidation;


namespace Demo.Console.Validators
{
    public class EnrolmentAttemptValidator : IEnrolmentAttemptValidator
    {
        public Validator IsEligible(string unitCode, string studentId)
        {
            // fake a db call and pretend student is not eligible.
            var calledDatabaseAndNotEligible = false;

            var validator = new Validator();

            validator.Validate("EnrolmentIssue", "Student is not eligible", calledDatabaseAndNotEligible);

            return validator;
        }

        public Validator IsEnrolled(string unitCode, string studentId)
        {
            // fake a db call and pretend student is already enrolled in unit.
            var calledDatabaseAndStudentIsAlreadyEnrolledInUsnit = true;

            var validator = new Validator();

            validator.Validate("EnrolmentIssue", "Student already enrolled", !calledDatabaseAndStudentIsAlreadyEnrolledInUsnit);

            return validator;
        }

        public Validator IsFinancial(string unitCode)
        {
            // fake a db call and pretend student is has not paid fees.
            var calledDatabaseAndStudentHasPaid = false;

            var validator = new Validator();

            validator.Validate("StudentFeesOwing", "Student has not paid fees", calledDatabaseAndStudentHasPaid);

            return validator;
        }
    }
}
