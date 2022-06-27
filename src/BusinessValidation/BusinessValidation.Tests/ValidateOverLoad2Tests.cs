using FluentAssertions;
using BusinessValidation.Tests.TestDomain;
using BusinessValidation.Tests.TestDomain.Builders;

namespace BusinessValidation.Tests
{
    public class ValidateOverLoad2Tests
    {
        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                ValidationInvariables.FailBundle.FirstName,
                ValidationInvariables.FailureMessage.NotRightNameErrorMessage,
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
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            validator.NotValid().Should().BeTrue();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            var isValid = validator.Validate(
                ValidationInvariables.FailBundle.Email,
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
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            isValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_Null_Object_Throws_ArgumentNullException()
        {
            var validator = new Validator();

            Lecturer bob = null;

            validator.Invoking(v => v.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Contains(LecturerBuilder.LecturerFirstName)
            )).Should()
            .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                string.Empty,
                ValidationInvariables.FailureMessage.FailMessageNameTooShort,
                bob,
                b => b.FirstName.Length > 4
                );

            validator.ValidationFailures[string.Empty].Should().Contain(ValidationInvariables.FailureMessage.FailMessageNameTooShort);
        }


        [Fact]
        public void Add_Null_FailBundle_Name_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Invoking(v =>
                v.Validate(
                    ValidationInvariables.FailBundle.NullFailBundle, 
                    ValidationInvariables.FailureMessage.FailMessageNameTooShort, 
                    bob, 
                    b => b.FirstName.Length > 4)
                ).Should()
                .Throw<ArgumentException>();
        }
                

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Invoking(v => v.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.MessageIsNullValue,
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
                ValidationInvariables.FailBundle.Email,
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
                ValidationInvariables.FailBundle.Email,
                string.Empty,
                bob,
                p => p.FirstName.Length > 4
            )).Should()
            .Throw<ArgumentException>();
        }
    }
}