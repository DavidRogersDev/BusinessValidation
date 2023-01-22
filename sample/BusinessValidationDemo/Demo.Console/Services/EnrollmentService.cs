using Demo.Console.Validators;
using System.Diagnostics;


namespace Demo.Console.Services
{
    /// <summary>
    /// In this sample service, validation is externalised to a separate validator class which is injected via its abstraction.
    /// </summary>
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

            var isValid = isEnrolledValidator
                .Merge(isEligibleValidator)
                .Merge(isFinancialValidator);

            IReadOnlyList<string> enrollmentIssues = isValid["EnrolmentIssue"]; // example of using the validator's indexer to access a particular fail-bundle
            Trace.WriteLine(enrollmentIssues[0]);
            Trace.WriteLine(enrollmentIssues[1]);

            isValid.ThrowIfInvalid();

            //  continue enrollment opeeration ...

            return isValid; // never reached, but required by compiler.
        }
    }
}
