using FluentAssertions;
using System;
using System.Linq;

namespace BusinessValidation.Tests
{
    public class MergeTests
    {
        private Validator validatorFirst;
        private Validator validatorSecond;
        private Validator validatorNull;
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

            ((bool)isValid).Should().BeFalse();
            isValid[UserNameFailBundleName].Should().Contain(UserNameEmptyMessage);
            isValid.ValidationMessages.Count.Should().Be(1);
        }
        
        [Fact]
        public void Merge_InValid_With_Valid_Returns_Merged()
        {
            var validator = new Validator();

            var isValid = validatorFirst.Merge(validator);

            ((bool)isValid).Should().BeFalse();
            isValid[UserNameFailBundleName].Should().Contain(UserNameEmptyMessage);
            isValid.ValidationMessages.Count.Should().Be(1);
        }
        
        [Fact]
        public void Merge_Valid_With_Valid_Returns_Merged()
        {
            var isValid = new Validator()
                .Merge(new Validator());

            ((bool)isValid).Should().BeTrue();
        }
        
        [Fact]
        public void Merge_Two_InValids_Returns_Merged()
        {
            var isValid = validatorFirst.Merge(validatorSecond);

            ((bool)isValid).Should().BeFalse();
            isValid.ValidationMessages.Count.Should().Be(3);
            isValid.ValidationFailures.Count.Should().Be(2);
            isValid[UserNameFailBundleName].Should().Contain(UserNameAlreadyTakenMessage);
            isValid[UserNameFailBundleName].Should().Contain(UserNameEmptyMessage);
            isValid[EmailFailBundleName].Should().Contain(EmailInvalidCharactersMessage);
        }
        
        [Fact]
        public void Merge_InValid_With_Null_Returns_Merged()
        {
            var isValid = validatorFirst.Merge(validatorNull);

            ((bool)isValid).Should().BeFalse();
        }

        [Fact]
        public void Merge_Valid_With_Null_Returns_Merged()
        {
            var isValid = new Validator().Merge(validatorNull);

            ((bool)isValid).Should().BeTrue();
        }

        [Fact]
        public void Chaining_Merges_Returns_Merged()
        {
            var isValid = validatorFirst
                .Merge(validatorNull)
                .Merge(validatorSecond);

            ((bool)isValid).Should().BeFalse();
            isValid.ValidationMessages.Count.Should().Be(3);
            isValid.ValidationFailures.Count.Should().Be(2);
            isValid[UserNameFailBundleName].Should().Contain(UserNameAlreadyTakenMessage);
            isValid[UserNameFailBundleName].Should().Contain(UserNameEmptyMessage);
            isValid[EmailFailBundleName].Should().Contain(EmailInvalidCharactersMessage);
        }
    }
}
