using FluentAssertions;
using BusinessValidation.Tests.TestDomain;
using BusinessValidation.Tests.TestDomain.Builders;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public class ValidateOverLoad3Tests
    {
        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                f => f.FirstName,
                FailureMessage.NotRightNameErrorMessage,
                bob,
                b => b.FirstName.Equals(LecturerBuilder.LecturerFirstName)
                );

            validator.IsValid().Should().BeTrue();
        }

        [Fact]
        public void Validate_InValid_Object_Fails_So_Not_Valid()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                f => f.EmailAddress,
                FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(GenericTestData.AnuUniSuffix)
                );

            validator.NotValid().Should().BeTrue();
        }

        [Fact]
        public void Validate_Null_Object_Throws_ArgumentNullException()
        {
            var validator = new Validator();

            Lecturer bob = null;

            validator.Invoking(v => v.Validate(
                f => f.EmailAddress,
                FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Contains(LecturerBuilder.LecturerFirstName)
            )).Should()
            .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Invoking(v => v.Validate(
                f => f.EmailAddress,
                FailureMessage.MessageIsNullValue,
                bob,
                p => p.FirstName.Length > 4
            )).Should()
            .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Invoking(v => v.Validate(
                b => b.EmailAddress,
                "      ",
                bob,
                p => p.FirstName.Length > 4
            )).Should()
            .Throw<ArgumentException>();
        }

        [Fact]
        public void Fail_Bundle_Token_Is_Replaced_By_PropertyName()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                p => p.FirstName,
                "{{fail-bundle}} should be longer than 3 characters.",
                bob,
                p => p.FirstName.Length > 3
                );

            validator[FailBundle.FirstName].Single().Should().Be($"FirstName {PartMessage.LongerThanThreeChars}");
        }

        [Fact]
        public void Fail_Bundle_Token_Is_Replaced_By_PropertyName_Where_Interpolation_Used()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                f => f.FirstName,
                $"The {{fail-bundle}} '{bob.FirstName}' {PartMessage.LongerThanThreeChars}",
                bob,
                p => p.FirstName.Length > 3
                );

            validator[FailBundle.FirstName].Single().Should().Be($"The FirstName 'Bob' {PartMessage.LongerThanThreeChars}");
        }
    }
}
