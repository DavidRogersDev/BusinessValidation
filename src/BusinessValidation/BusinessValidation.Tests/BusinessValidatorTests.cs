using BusinessValidation.Tests.Generators;
using BusinessValidation.Tests.IntegrationTestClasses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessValidation.Tests
{
    public class BusinessValidatorTests
    {
        private readonly Assembly _assembly;
        private readonly IServiceCollection _services;

        public BusinessValidatorTests()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _services = new ServiceCollection();
        }

        [Fact]
        public void G()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            Subject subject = new Subject { Id = 1, Name = StringGenerator.GetString(99) };

            BusinessValidationResult result = subjectValidator.Validate(subject);

            result.IsValid.ShouldBeTrue();
        }
        
        [Fact]
        public void H()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            Subject subject = new Subject { Id = 1, Name = StringGenerator.GetString2(100) };

            BusinessValidationResult result = subjectValidator.Validate(subject);            

            result.NotValid.ShouldBeTrue();
            result.ValidationFailures.SelectMany(f => f.Value).ShouldContain("Name must be less than 100 characters long.");
        }
    }
}
