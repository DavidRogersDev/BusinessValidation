namespace BusinessValidation.Tests
{
    internal sealed class ValidationInvariables
    {

        internal sealed class GenericTestData
        {
            internal const string AnuUniSuffix = "anu.edu.au";
            internal const string VeryShortName = "Jo";
        }

        internal sealed class FailBundle
        {
            internal const string Name = "Name";
            internal const string Email = "Email";
            internal const string FirstName = "FirstName";
            internal const string Age = "Age";
            internal static readonly string NullFailBundle = null;
        }
        
        internal sealed class PartMessage
        {
            internal static string LongerThanThreeChars = "should be longer than 3 characters.";
        }

        internal sealed class FailureMessage
        {
            internal const string FailMessageNameTooShort = "Name too short";
            internal static readonly string MessageIsNullValue = null;
            internal const string NotRightNameEmail= "Not the right email";
            internal const string NotRightNameErrorMessage = "Not the right name";
            internal const string PersonTooYoung = "Person is too young";
        }
    }
}
