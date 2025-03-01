﻿namespace BusinessValidation.Tests
{
    internal static class ValidationInvariables
    {

        internal static class GenericTestData
        {
            internal const string AnuUniSuffix = "anu.edu.au";
            internal const string VeryShortName = "Jo";
        }

        internal static class FailBundle
        {
            internal const string AddressPostcode = "Address.PostCode";
            internal const string Age = "Age";
            internal const string Email = "Email";
            internal const string FirstName = "FirstName";
            internal const string Name = "Name";
            internal static readonly string NullFailBundle = null;
            internal const string PostCode = "PostCode";
        }

        internal static class PartMessage
        {
            internal const string LongerThanThreeChars = "should be longer than 3 characters.";
        }

        internal static class FailureMessage
        {
            internal const string FailMessageNameTooShort = "Name too short";
            internal static readonly string MessageIsNullValue = null;
            internal const string NotRightNameEmail = "Not the right email";
            internal const string NotRightNameErrorMessage = "Not the right name";
            internal const string PersonTooYoung = "Person is too young";
            internal const string PostCodeTooFar = "Postcode is too far";
        }
    }
}
