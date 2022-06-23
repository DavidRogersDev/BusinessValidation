using Demo.Console.Validators;
using System.Diagnostics;


namespace Demo.Console.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrolmentAttemptValidator _enrolmentAttemptValidator;

        public EnrollmentService(IEnrolmentAttemptValidator enrolmentAttemptValidator)
        {
            _enrolmentAttemptValidator = enrolmentAttemptValidator;
        }

        public bool EnrolInUnit(string unitCode, string studentId)
        {
            var isEnrolledValidator = _enrolmentAttemptValidator.IsEnrolled(unitCode, studentId);
            var isEligibleValidator = _enrolmentAttemptValidator.IsEligible(unitCode, studentId);
            var isFinancialValidator = _enrolmentAttemptValidator.IsFinancial(studentId);

            isEligibleValidator.AddFailure("EnrolmentIssue", "Student is not eligible")
                .AddFailure("EnrolmentIssue", "Student already enrolled")
                .AddFailure("StudentFeesOwing", "Student has not paid fees");
                ;

            var isValid = isEnrolledValidator
                .Merge(isEligibleValidator)
                .Merge(isFinancialValidator);

            if(isValid)
            {
                //  continue enrollment opeeration ...
            }

            isValid.Throw();

            return false; // never reached, but required by compiler.
        }

        private bool IsEnrolled(string unitCode, string studentId)
        {
            // fake a db call and pretend student is already enrolled in unit.
            return true;
        }
        
        private bool IsEligible(string unitCode, string studentId)
        {
            // fake a db call and pretend student is not eligible.
            return false;
        }

        private bool IsFinancial(string studentId)
        {
            // fake a db call and pretend student is not eligible.
            return false;
        }
    }
}
