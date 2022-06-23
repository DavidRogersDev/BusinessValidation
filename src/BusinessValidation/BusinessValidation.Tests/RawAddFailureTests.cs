using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using static BusinessValidation.Tests.ValidationInvariables;

namespace BusinessValidation.Tests
{
    public class RawAddFailureTests
    {
        private const string NotRightNameErrorMessage = "Not the right name";
        private const string FailBundleName = "Name";
        private const string FailBundleAge = "Age";
        private const string FailMessageNameTooShort = "Name too short";

        [Fact]
        public void Validator_Should_Have_Inner_Failure_Collection()
        {
            var validator = new Validator();

            validator.ValidationFailures.Should().NotBeNull();
            validator.ValidationFailures.Count.Should().Be(0);
            ((bool)validator).Should().BeTrue();
        }
        
        [Fact]
        public void Validator_Should_Have_Inner_Failure_Messages()
        {
            var validator = new Validator();

            validator.ValidationMessages.Should().NotBeNull();
            validator.ValidationMessages.Count().Should().Be(0);
        }
        
        [Fact]
        public void New_Validator_Should_Be_Valid()
        {
            var validator = new Validator();

            validator.IsValid().Should().BeTrue();
        }

        [Fact]
        public void Add_Failure_Should_Make_IsValid_False()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.IsValid().Should().BeFalse();
        }
        
        [Fact]
        public void Add_Failure_Should_Make_NotValid_True()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.NotValid().Should().BeTrue();
        }
        
        [Fact]
        public void Add_Failure_Should_Add_Fail_Bundle_Name()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.ValidationFailures.Should().ContainKey(FailBundleName);
        }
        
        [Fact]
        public void Add_Failure_Should_Add_FailMessage_To_Bundle()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.ValidationFailures
                .Where(b => b.Key == FailBundleName)
                .SelectMany(v => v.Value)
                .Single()
                .Should()
                .Be(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Failure_Should_Be_Available_As_Part_Of_Messages_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.ValidationMessages
                .Single()
                .Should()
                .Be(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Failure_Should_Add_Failure_To_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationFailures.Count.Should().BeGreaterThan(0);
        }
        
        [Fact]
        public void ValidationMessages_Should_Equal_Nr_Messages_Added()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationMessages.Count().Should().Be(2);
        }
        
        [Fact]
        public void Failures_With_Same_Key_Added_To_Same_Bundle()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleName, FailMessageNameTooShort);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationFailures[FailBundleName].Should().Contain(FailMessageNameTooShort).And.Contain(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            validator.AddFailure(string.Empty, NotRightNameErrorMessage);
            validator.AddFailure(string.Empty, FailMessageNameTooShort);           

            validator.ValidationFailures[string.Empty].Should().Contain(FailMessageNameTooShort).And.Contain(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Null_FailBundle_Throws_Exception()
        {
            var validator = new Validator();

            validator.Invoking(v => v.AddFailure(ValidationInvariables.FailBundle.NullFailBundle, NotRightNameErrorMessage))
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            string message = null;

            validator.Invoking(v => v.AddFailure(FailBundleName, message))
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            string message = string.Empty;

            validator.Invoking(v => v.AddFailure(FailBundleName, message))
                .Should()
                .Throw<ArgumentException>();
        }
        
        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            string message = "       ";

            validator.Invoking(v => v.AddFailure(FailBundleName, message))
                .Should()
                .Throw<ArgumentException>();
        }
        
        [Fact]
        public void Add_Failure_Returns_Instance()
        {
            var validator = new Validator();

            var val = validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            val.Should().NotBeNull();
            val.Should().BeSameAs(validator);
        }
    }
}
