namespace BusinessValidation.Tests
{
    public class MergeTests
    {
        private Validator validatorFirst;
        private Validator validatorSecond;
        private Validator validatorNull = null;
        const string UserNameEmptyMessage = "UserName was empty";
        const string UserNameAlreadyTakenMessage = "UserName was already taken";
        const string UserNameFailBundleName = "UserName";
        const string EmailInvalidCharactersMessage = "Email contains invalid characters";
        const string EmailFailBundleName = "Email";

        public MergeTests()
        {
            validatorFirst = new Validator();

            validatorFirst.AddFailure(UserNameFailBundleName, UserNameEmptyMessage);

            validatorSecond = new Validator();
            validatorSecond.AddFailure(UserNameFailBundleName, UserNameAlreadyTakenMessage)
                .AddFailure(EmailFailBundleName, EmailInvalidCharactersMessage);
        }

        [Fact]
        public void Merge_Valid_With_InValid_Returns_Merged()
        {
            var validator = new Validator();

            var isValid = validator.Merge(validatorFirst);

            ((bool)isValid).ShouldBeFalse();
            isValid[UserNameFailBundleName].ShouldContain(UserNameEmptyMessage);
            isValid.ValidationMessages.Count.ShouldBe(1);
        }
        
        [Fact]
        public void Merge_InValid_With_Valid_Returns_Merged()
        {
            var validator = new Validator();

            var isValid = validatorFirst.Merge(validator);

            ((bool)isValid).ShouldBeFalse();
            isValid[UserNameFailBundleName].ShouldContain(UserNameEmptyMessage);
            isValid.ValidationMessages.Count.ShouldBe(1);
        }
        
        [Fact]
        public void Merge_Valid_With_Valid_Returns_Merged()
        {
            var isValid = new Validator()
                .Merge(new Validator());

            ((bool)isValid).ShouldBeTrue();
        }
        
        [Fact]
        public void Merge_Two_InValids_Returns_Merged()
        {
            var isValid = validatorFirst.Merge(validatorSecond);

            ((bool)isValid).ShouldBeFalse();
            isValid.ValidationMessages.Count.ShouldBe(3);
            isValid.ValidationFailures.Count.ShouldBe(2);
            isValid[UserNameFailBundleName].ShouldContain(UserNameAlreadyTakenMessage);
            isValid[UserNameFailBundleName].ShouldContain(UserNameEmptyMessage);
            isValid[EmailFailBundleName].ShouldContain(EmailInvalidCharactersMessage);
        }
        
        [Fact]
        public void Merge_InValid_With_Null_Returns_Merged()
        {
            var isValid = validatorFirst.Merge(validatorNull);

            ((bool)isValid).ShouldBeFalse();
        }

        [Fact]
        public void Merge_Valid_With_Null_Returns_Merged()
        {
            var isValid = new Validator().Merge(validatorNull);

            ((bool)isValid).ShouldBeTrue();
        }

        [Fact]
        public void Chaining_Merges_Returns_Merged()
        {
            var isValid = validatorFirst
                .Merge(validatorNull)
                .Merge(validatorSecond);

            ((bool)isValid).ShouldBeFalse();
            isValid.ValidationMessages.Count.ShouldBe(3);
            isValid.ValidationFailures.Count.ShouldBe(2);
            isValid[UserNameFailBundleName].ShouldContain(UserNameAlreadyTakenMessage);
            isValid[UserNameFailBundleName].ShouldContain(UserNameEmptyMessage);
            isValid[EmailFailBundleName].ShouldContain(EmailInvalidCharactersMessage);
        }
    }
}
