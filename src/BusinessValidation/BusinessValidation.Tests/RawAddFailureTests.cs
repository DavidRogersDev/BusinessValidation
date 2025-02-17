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

            validator.ValidationFailures.ShouldNotBeNull();
            validator.ValidationFailures.Count.ShouldBe(0);
            ((bool)validator).ShouldBeTrue();
        }
        
        [Fact]
        public void Validator_Should_Have_Inner_Failure_Messages()
        {
            var validator = new Validator();

            validator.ValidationMessages.ShouldNotBeNull();
            validator.ValidationMessages.Count().ShouldBe(0);
        }
        
        [Fact]
        public void New_Validator_Should_Be_Valid()
        {
            var validator = new Validator();

            validator.IsValid().ShouldBeTrue();
        }

        [Fact]
        public void Add_Failure_Should_Make_IsValid_False()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.IsValid().ShouldBeFalse();
            ((bool)validator).ShouldBeFalse();
        }
        
        [Fact]
        public void Add_Failure_Should_Make_NotValid_True()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.NotValid().ShouldBeTrue();
        }
        
        [Fact]
        public void Add_Failure_Should_Add_Fail_Bundle_Name()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.ValidationFailures.ShouldContainKey(FailBundleName);
        }
        
        [Fact]
        public void Failure_Should_Be_Accessible_By_Indexer()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator[FailBundleName].Count.ShouldBe(1);
            validator[FailBundleName].ShouldContain(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Failure_Should_Add_FailMessage_To_Bundle()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator[FailBundleName]                
                .Single()
                .ShouldBe(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Failure_Should_Be_Available_As_Part_Of_Messages_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            validator.ValidationMessages
                .Single()
                .ShouldBe(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Failure_Should_Add_Failure_To_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationFailures.Count.ShouldBeGreaterThan(0);
        }
        
        [Fact]
        public void ValidationMessages_Should_Equal_Nr_Messages_Added()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationMessages.Count().ShouldBe(2);
            validator.ValidationMessages.ShouldContain(NotRightNameErrorMessage);
            validator.ValidationMessages.ShouldContain(FailureMessage.PersonTooYoung);
        }
        
        [Fact]
        public void Failures_With_Same_Key_Added_To_Same_Bundle()
        {
            var validator = new Validator();

            validator.AddFailure(FailBundleName, NotRightNameErrorMessage);
            validator.AddFailure(FailBundleName, FailMessageNameTooShort);
            validator.AddFailure(FailBundleAge, FailureMessage.PersonTooYoung);

            validator.ValidationFailures[FailBundleName].ShouldContain(FailMessageNameTooShort);
            validator.ValidationFailures[FailBundleName].ShouldContain(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Empty_FailBundle_Name_Is_Fine()
        {
            var validator = new Validator();

            validator.AddFailure(string.Empty, NotRightNameErrorMessage);
            validator.AddFailure(string.Empty, FailMessageNameTooShort);           

            validator.ValidationFailures[string.Empty].ShouldContain(FailMessageNameTooShort);
            validator.ValidationFailures[string.Empty].ShouldContain(NotRightNameErrorMessage);
        }
        
        [Fact]
        public void Add_Null_FailBundle_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentNullException>(() => validator.AddFailure(FailBundle.NullFailBundle, NotRightNameErrorMessage));
        }
        
        [Fact]
        public void Add_Null_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            Should.Throw<ArgumentNullException>(() => validator.AddFailure(FailBundleName, FailureMessage.MessageIsNullValue));
        }
        
        [Fact]
        public void Add_Empty_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            string message = string.Empty;

            Should.Throw<ArgumentException>(() => validator.AddFailure(FailBundleName, message));
        }
        
        [Fact]
        public void Add_WhiteSpace_FailMessage_Throws_Exception()
        {
            var validator = new Validator();

            string message = "       ";

            Should.Throw<ArgumentException>(() => validator.AddFailure(FailBundleName, message));
        }
        
        [Fact]
        public void Add_Failure_Returns_Instance()
        {
            var validator = new Validator();

            var val = validator.AddFailure(FailBundleName, NotRightNameErrorMessage);

            val.ShouldNotBeNull();
            val.ShouldBeSameAs(validator);
        }
    }
}
