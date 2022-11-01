using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessValidation.Tests
{
    public class ThrowIfInvalidTests
    {
        [Fact]
        public void ThrowIfInvalid_Throws_Exception()
        {
            var validator = new Validator();

            validator.AddFailure(ValidationInvariables.FailBundle.Name, ValidationInvariables.FailureMessage.FailMessageNameTooShort);

            validator.Invoking(v => v.ThrowIfInvalid())
                .Should()
                .Throw<ValidationFailureException>();
        }
        
        [Fact]
        public void ThrowIfInvalid_Does_Not_Throw_Exception_When_Valid()
        {
            var validator = new Validator();

            validator.Invoking(v => v.ThrowIfInvalid())
                .Should()
                .NotThrow<ValidationFailureException>();
        }


        [Fact]
        public void ThrowIfInvalid_Throws_Exception_With_ValidationFailures_Collection()
        {
            var validator = new Validator();

            validator.AddFailure(ValidationInvariables.FailBundle.Name, ValidationInvariables.FailureMessage.FailMessageNameTooShort);

            validator.Invoking(v => v.ThrowIfInvalid())
                .Should()
                .Throw<ValidationFailureException>()
                .Which
                .Failures
                .Should()
                .HaveCount(1)
                .And
                .ContainKey(ValidationInvariables.FailBundle.Name);
        }
    }
}
