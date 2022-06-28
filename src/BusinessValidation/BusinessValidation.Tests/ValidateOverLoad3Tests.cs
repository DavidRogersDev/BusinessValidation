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
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            var isValid = validator.Validate(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Equals(LecturerBuilder.LecturerFirstName)
                );

            isValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            var isValid = validator.Validate(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            isValid.Should().BeFalse();
        }


        [Fact]
        public void Full_Path_Added_As_Failbundle_Name_As_Default()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.SimpleWithAddress().Build();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000
                );

            validator.ValidationFailures.Single().Key.Should().Be(FailBundle.AddressPostcode);
        }

        [Fact]
        public void Fullpath_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.SimpleWithAddress().Build();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000,
                PropertyDepth.FullPath
                );

            validator.ValidationFailures.Single().Key.Should().Be(FailBundle.AddressPostcode);
        }

        [Fact]
        public void TerminatingProperty_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.SimpleWithAddress().Build();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000,
                PropertyDepth.TerminatingProperty
                );

            validator.ValidationFailures.Single().Key.Should().Be(FailBundle.PostCode);
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
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Invoking(v => v.Validate(
                b => b.EmailAddress,
                string.Empty,
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
