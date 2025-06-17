using BusinessValidation.Tests.TestDomain;

using BusinessValidation.Tests.TestDomain.Generators;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public sealed class ValidateOverLoad3Tests
    {
        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                f => f.FirstName,
                FailureMessage.NotRightNameErrorMessage,
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
                f => f.EmailAddress,
                FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(GenericTestData.AnuUniSuffix)
                );

            validator.NotValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            var isValid = validator.Validate(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.Equals(LecturerBuilder.LecturerEmail)
                );

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            var isValid = validator.Validate(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob,
                b => b.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            isValid.ShouldBeFalse();
        }


        [Fact]
        public void Full_Path_Added_As_Failbundle_Name_As_Default()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimpleWithAddress();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000
                );

            validator.ValidationFailures.Single().Key.ShouldBe(FailBundle.AddressPostcode);
        }

        [Fact]
        public void Fullpath_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimpleWithAddress();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000,
                PropertyDepth.FullPath
                );

            validator.ValidationFailures.Single().Key.ShouldBe(FailBundle.AddressPostcode);
        }

        [Fact]
        public void TerminatingProperty_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimpleWithAddress();

            validator.Validate(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob,
                b => b.Address.PostCode > 5000,
                PropertyDepth.TerminatingProperty
                );

            validator.ValidationFailures.Single().Key.ShouldBe(FailBundle.PostCode);
        }

        [Fact]
        public void Validate_Null_Object_Throws_ArgumentNullException()
        {
            var validator = new Validator();

            Lecturer bob = null;

            Should.Throw<ArgumentNullException>(() => validator.Validate(
                f => f.EmailAddress,
                FailureMessage.NotRightNameEmail,
                bob,
                b => b.FirstName.Contains(LecturerBuilder.LecturerFirstName)));

        }

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            Should.Throw<ArgumentNullException>(() => validator.Validate(
                f => f.EmailAddress,
                FailureMessage.MessageIsNullValue,
                bob,
                p => p.FirstName.Length > 4));
        }

        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            Should.Throw<ArgumentException>(() => validator.Validate(
                b => b.EmailAddress,
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
                b => b.EmailAddress,
                string.Empty,
                bob,
                p => p.FirstName.Length > 4));
        }

        [Fact]
        public void Fail_Bundle_Token_Is_Replaced_By_PropertyName()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                p => p.FirstName,
                "{fail-bundle} should be longer than 3 characters.",
                bob,
                p => p.FirstName.Length > 3
                );

            validator[FailBundle.FirstName].Single().ShouldBe($"FirstName {PartMessage.LongerThanThreeChars}");
        }

        [Fact]
        public void Fail_Bundle_Token_Is_Replaced_By_PropertyName_Where_Interpolation_Used()
        {
            var validator = new Validator();

            var bob = LecturerGenerator.GenerateSimple();

            validator.Validate(
                f => f.FirstName,
                $"The {{fail-bundle}} '{bob.FirstName}' {PartMessage.LongerThanThreeChars}",
                bob,
                p => p.FirstName.Length > 3
                );

            validator[FailBundle.FirstName].Single().ShouldBe($"The FirstName 'bob' {PartMessage.LongerThanThreeChars}");
        }
    }
}
