using BusinessValidation.Tests.Generators;
using BusinessValidation.Tests.IntegrationTestClasses;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BusinessValidation.Tests
{
    public class BusinessValidatorTests
    {
        private const string StudentErrorMessage = "Id must have a positive value.";
        private const string SubjectErrorMessage = "Name must be less than 100 characters long.";
        private readonly Assembly _assembly;
        private readonly IServiceCollection _services;

        public BusinessValidatorTests()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _services = new ServiceCollection();
        }

        [Fact]
        public void BusinessValidator_Returns_IsValid_When_Validation_Passes()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            Subject subject = new Subject { Id = 1, Name = StringGenerator.GetString(99) };

            BusinessValidationResult result = subjectValidator.Validate(subject);

            result.IsValid.ShouldBeTrue();
        }
        
        [Fact]
        public void BusinessValidator_Returns_NotValid_When_Validation_Fails()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            Subject subject = new Subject { Id = 1, Name = StringGenerator.GetString2(100) };

            BusinessValidationResult result = subjectValidator.Validate(subject);            

            result.NotValid.ShouldBeTrue();
            result.ValidationFailures.SelectMany(f => f.Value).ShouldContain(SubjectErrorMessage );
        }
        
        [Fact]
        public void Combined_BusinessValidator_Returns_NotValid_When_Validation_Fails()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetServices<IBusinessValidator<Subject>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));
            IBusinessValidator<Student> studentValidator = container.GetServices<IBusinessValidator<Student>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));

            Subject invalidSubject = new Subject { Id = 1, Name = StringGenerator.GetString2(100) };

            BusinessValidationResult subjectValidatorResult = subjectValidator.Validate(invalidSubject);
            BusinessValidationResult studentValidatorResult = studentValidator.Validate(new Student());

            subjectValidatorResult.NotValid.ShouldBeTrue();
            subjectValidatorResult.ValidationFailures.SelectMany(f => f.Value).ShouldContain(SubjectErrorMessage );

            studentValidatorResult.NotValid.ShouldBeTrue();
            studentValidatorResult.ValidationFailures.SelectMany(f => f.Value).ShouldContain(StudentErrorMessage);
        }
        
        [Fact]
        public void Combined_BusinessValidator_Returns_NotValid_When_BusinessValidationResult_Merged()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetServices<IBusinessValidator<Subject>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));
            IBusinessValidator<Student> studentValidator = container.GetServices<IBusinessValidator<Student>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));

            Subject invalidSubject = new Subject { Id = 1, Name = StringGenerator.GetString2(100) };

            BusinessValidationResult subjectValidatorResult = subjectValidator.Validate(invalidSubject);
            BusinessValidationResult studentValidatorResult = studentValidator.Validate(new Student());

            BusinessValidationResult mergedResult = subjectValidatorResult.Merge(studentValidatorResult);

            mergedResult.NotValid.ShouldBeTrue();
            mergedResult.ValidationFailures.SelectMany(f => f.Value).ShouldContain(StudentErrorMessage);
            mergedResult.ValidationFailures.SelectMany(f => f.Value).ShouldContain(SubjectErrorMessage);
        }
    }
}
