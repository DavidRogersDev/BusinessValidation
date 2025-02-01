namespace BusinessValidation.Tests
{
    public class ThrowIfInvalidTests
    {
        [Fact]
        public void ThrowIfInvalid_Throws_Exception()
        {
            var validator = new Validator();

            validator.AddFailure(ValidationInvariables.FailBundle.Name, ValidationInvariables.FailureMessage.FailMessageNameTooShort);

            Should.Throw<ValidationFailureException>(() => validator.ThrowIfInvalid());
        }
        
        [Fact]
        public void ThrowIfInvalid_Does_Not_Throw_Exception_When_Valid()
        {
            var validator = new Validator();

            Should.NotThrow(() => validator.ThrowIfInvalid());
        }


        [Fact]
        public void ThrowIfInvalid_Throws_Exception_With_ValidationFailures_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(ValidationInvariables.FailBundle.Name, ValidationInvariables.FailureMessage.FailMessageNameTooShort);

            Should.Throw<ValidationFailureException>(() => validator.ThrowIfInvalid());
            validator.ValidationFailures.Count.ShouldBe(1);
            validator.ValidationFailures.ShouldContainKey(ValidationInvariables.FailBundle.Name);
        }
    }
}
