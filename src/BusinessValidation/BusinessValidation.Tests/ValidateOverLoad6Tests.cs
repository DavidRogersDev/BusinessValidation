using BusinessValidation.Tests.TestDomain;
using BusinessValidation.Tests.TestDomain.Builders;
using System;
using System.Linq;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public sealed class ValidateOverLoad6Tests
    {
        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate<Lecturer, string>(
                f => f.FirstName,
                FailureMessage.NotRightNameErrorMessage,
                bob.FirstName.Equals(LecturerBuilder.LecturerFirstName)
                );

            validator.IsValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_InValid_Object_Fails_So_Not_Valid()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate<Lecturer, bool>(
                f => f.CurrentlyRostered,
                FailureMessage.NotRightNameEmail,
                bob.CurrentlyRostered == false
                );

            validator.NotValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            var isValid = validator.Validate<Lecturer, string>(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob.EmailAddress.Equals(LecturerBuilder.LecturerEmail)
                );

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            var isValid = validator.Validate<Lecturer, string>(
                l => l.EmailAddress,
                ValidationInvariables.FailureMessage.NotRightNameEmail,
                bob.EmailAddress.EndsWith(ValidationInvariables.GenericTestData.AnuUniSuffix)
                );

            isValid.ShouldBeFalse();
        }

        [Fact]
        public void Fullpath_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.SimpleWithAddress().Build();

            var isValid = validator.Validate<Lecturer, int>(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob.Address.PostCode > 5000,
                PropertyDepth.FullPath
                );

            validator.ValidationFailures.Single().Key.ShouldBe(FailBundle.AddressPostcode);
        }

        [Fact]
        public void TerminatingProperty_Enum_Adds_Full_Path_As_Failbundle_Name()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.SimpleWithAddress().Build();

            var isValid = validator.Validate<Lecturer, int>(
                l => l.Address.PostCode,
                FailureMessage.PostCodeTooFar,
                bob.Address.PostCode > 5000,
                PropertyDepth.TerminatingProperty
                );

            validator.ValidationFailures.Single().Key.ShouldBe(FailBundle.PostCode);
        }
    }
}
