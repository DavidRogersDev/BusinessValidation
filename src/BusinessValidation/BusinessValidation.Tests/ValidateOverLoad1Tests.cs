using FluentAssertions;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public class ValidateOverLoad1Tests 
    {

        [Fact]
        public void Validate_Adds_Failure()
        {
            var validator = new Validator();
            
            validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            validator.NotValid().Should().BeTrue();
            validator.ValidationMessages.Should().Contain(FailureMessage.FailMessageNameTooShort, because: "message should be present in Failure Messages collection.");
            validator[FailBundle.Name].Should().Contain(FailureMessage.FailMessageNameTooShort, because: $"message should be present in messages of '{FailBundle.Name}' fail bundle.");
        }

        [Fact]
        public void Validate_Passes_So_No_Failure_Added()
        {
            var validator = new Validator();

            validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length < 3);

            validator.IsValid().Should().BeTrue();
            validator.ValidationMessages.Should().BeEmpty();
            validator.ValidationFailures.Should().BeEmpty();
        }
        
        [Fact]
        public void Validate_Returns_True_When_Passes()
        {
            var validator = new Validator();

            var isValid = validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length < 3);

            isValid.Should().BeTrue();
        }
        
        [Fact]
        public void Validate_Returns_False_When_Fails()
        {
            var validator = new Validator();

            var isValid = validator.Validate(FailBundle.Name, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            validator.Validate(string.Empty, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2);

            validator.ValidationFailures[string.Empty].Should().Contain(FailureMessage.FailMessageNameTooShort);
        }


        [Fact]
        public void Add_Null_FailBundle_Name_Throws_Exception()
        {
            var validator = new Validator();

            validator.Invoking(
                v => v.Validate(FailBundle.NullFailBundle, FailureMessage.FailMessageNameTooShort, GenericTestData.VeryShortName.Length > 2)
                ).Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            validator.Invoking(v => v.Validate(
                FailBundle.Email,
                null,
                GenericTestData.VeryShortName.Length > 2
            )).Should()
            .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            validator.Invoking(v => v.Validate(
                FailBundle.Email,
                "      ",
                GenericTestData.VeryShortName.Length > 2
            )).Should()
            .Throw<ArgumentException>();
        }
    }
}