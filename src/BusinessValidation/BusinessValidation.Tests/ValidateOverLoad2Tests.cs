using BusinessValidation.Tests.TestDomain;

using BusinessValidation.Tests.TestDomain.Generators;

namespace BusinessValidation.Tests
{
    public sealed class ValidateOverLoad2Tests
    {
        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                ValidationInvariables.FailBundle.FirstName,
                ValidationInvariables.FailureMessage.NotRightNameErrorMessage,
                bob,
                b => b.FirstName.Equals(LecturerBuilder.LecturerFirstName)
                );

            validator.IsValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_InValid_Object_Fails_So_Not_Valid()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            validator.NotValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            var isValid = validator.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Equals(LecturerBuilder.LecturerFirstName)
                );

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            var isValid = validator.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            isValid.ShouldBeFalse();
        }

        [Fact]
        public void Validate_Null_Object_Throws_ArgumentNullException()
        {
            var validator = new Validator();

            Lecturer bob = null;

            Should.Throw<ArgumentNullException>(() => validator.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Contains(LecturerBuilder.LecturerFirstName)));

        }

        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                string.Empty,
                ValidationInvariables.FailureMessage.FailMessageNameTooShort,
                bob,
                b => b.FirstName.Length > 4
                );

            validator.ValidationFailures[string.Empty].ShouldContain(ValidationInvariables.FailureMessage.FailMessageNameTooShort);
        }


        [Fact]
        public void Add_Null_FailBundle_Name_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            Should.Throw<ArgumentException>(() => validator.Validate(
                    ValidationInvariables.FailBundle.NullFailBundle,
                    ValidationInvariables.FailureMessage.FailMessageNameTooShort,
                    bob,
                    b => b.FirstName.Length > 4));
        }
                

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            Should.Throw<ArgumentNullException>(() => validator.Validate(
                ValidationInvariables.FailBundle.Email,
                ValidationInvariables.FailureMessage.MessageIsNullValue,
                bob,
                p => p.FirstName.Length > 4));

        }

        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();


            Should.Throw<ArgumentException>(() => validator.Validate(
                ValidationInvariables.FailBundle.Email,
                "      ",
                bob,
                p => p.FirstName.Length > 4));
        }
        
        [Fact]
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            Should.Throw<ArgumentException>(() => validator.Validate(
                ValidationInvariables.FailBundle.Email,
                string.Empty,
                bob,
                p => p.FirstName.Length > 4));

        }
    }
}