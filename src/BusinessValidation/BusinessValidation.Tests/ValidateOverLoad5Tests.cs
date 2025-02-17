using BusinessValidation.Tests.TestDomain.Builders;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public sealed class ValidateOverLoad5Tests
    {
        private const string FirstNameKey = "FirstName";

        [Fact]
        public void Validate_Valid_Object_With_Predicate_Passes()
        {
            var validator = new Validator();

            var bob = LecturerBuilder.Simple().Build();

            validator.Validate(
                FirstNameKey,
                FailureMessage.NotRightNameErrorMessage,
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.FirstName.Equals(LecturerBuilder.LecturerFirstName);
                });

            validator.IsValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_InValid_Object_Fails_So_Not_Valid()
        {
            var validator = new Validator();

            validator.Validate(
                "CurrentlyRostered",
                FailureMessage.NotRightNameEmail,
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.CurrentlyRostered.Equals(false);
                });

            validator.NotValid().ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var isValid = validator.Validate(
                FirstNameKey,
                FailureMessage.NotRightNameEmail,
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.FirstName.Equals(LecturerBuilder.LecturerFirstName);
                });

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var isValid = validator.Validate(
                "EmailAddress",
                FailureMessage.NotRightNameEmail,
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.EmailAddress.EndsWith(GenericTestData.AnuUniSuffix);
                });

            isValid.ShouldBeFalse();
        }


        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentNullException>(() => validator.Validate(
                FirstNameKey,
                FailureMessage.MessageIsNullValue,
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.FirstName.Length > 4;
                }));
        }

        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentException>(() => validator.Validate(
                FirstNameKey,
                "      ",
                () =>
                {
                    var bob = LecturerBuilder.Simple().Build();
                    return bob.FirstName.Length > 4;
                }));
        }

        [Fact]
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentException>(() => validator.Validate(
                FirstNameKey,
                string.Empty,
            () =>
            {
                var bob = LecturerBuilder.Simple().Build();
                return bob.FirstName.Length > 4;
            }));

        }
    }
}
