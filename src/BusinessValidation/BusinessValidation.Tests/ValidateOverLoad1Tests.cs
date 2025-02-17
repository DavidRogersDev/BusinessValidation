using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public sealed class ValidateOverLoad1Tests
    {

        [Fact]
        public void Validate_Adds_Failure()
        {
            var validator = new Validator();

            validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            validator.NotValid().ShouldBeTrue();
            validator.ValidationMessages.ShouldContain(FailureMessage.FailMessageNameTooShort, "message should be present in Failure Messages collection.");
            validator[FailBundle.Name].ShouldContain(FailureMessage.FailMessageNameTooShort, $"message should be present in messages of '{FailBundle.Name}' fail bundle.");
        }

        [Fact]
        public void Validate_Passes_So_No_Failure_Added()
        {
            var validator = new Validator();

            validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length < 3);

            validator.IsValid().ShouldBeTrue();
            validator.ValidationMessages.ShouldBeEmpty();
            validator.ValidationFailures.ShouldBeEmpty();
        }

        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var isValid = validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length < 3);

            isValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var isValid = validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            isValid.ShouldBeFalse();
        }

        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            validator.Validate(string.Empty, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            validator.ValidationFailures[string.Empty].ShouldContain(FailureMessage.FailMessageNameTooShort);
        }


        [Fact]
        public void Add_Null_FailBundle_Name_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentNullException>(() => validator.Validate(FailBundle.NullFailBundle, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2));
        }

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentNullException>(() => validator.Validate(FailBundle.Email, null, GenericTestData.VeryShortName.Length > 2));

        }

        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentException>(() => validator.Validate(FailBundle.Email, "      ", GenericTestData.VeryShortName.Length > 2));
        }

        [Fact]
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentException>(() => validator.Validate(FailBundle.Email, string.Empty, GenericTestData.VeryShortName.Length > 2));
        }
    }
}