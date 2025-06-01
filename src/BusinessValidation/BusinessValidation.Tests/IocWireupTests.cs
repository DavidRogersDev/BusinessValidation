using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BusinessValidation.Tests.IntegrationTestClasses;
using System.ComponentModel;


namespace BusinessValidation.Tests
{
    public class IocWireupTests
    {
        private readonly Assembly _assembly;
        private readonly IServiceCollection _services;

        public IocWireupTests()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _services = new ServiceCollection();
        }

        [Fact]
        public void Invoking_Add_On_Assembly_Adds_Registration()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            subjectValidator.ShouldNotBeNull();
        }

        [Fact]
        public void Invoking_Add_On_Assemblies_Adds_Registration()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions))!;

            IServiceCollection services = _services.AddBusinessValidation(new[] { assembly, _assembly });
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetRequiredService<IBusinessValidator<Subject>>();

            subjectValidator.ShouldNotBeNull();
        }

        [Fact]
        public void Invoking_Add_On_Assembly_Registers_All_Implementations()
        {
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            
            services.Count.ShouldBe(4);
        }

        [Fact]
        public void Invoking_Add_On_Assemblies_Registers_All_Implementations()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions))!;
            IServiceCollection services = _services.AddBusinessValidation(new[] { assembly, _assembly });

            services.Count.ShouldBe(4);
        }
        
        [Fact]
        public void Invoking_Add_On_Assemblies_Registers_Implementations_With_Combined_Validators()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions))!;
            IServiceCollection services = _services.AddBusinessValidation(new[] { assembly, _assembly });
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetServices<IBusinessValidator<Subject>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));
            IBusinessValidator<Student> studentValidator = container.GetServices<IBusinessValidator<Student>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));

            subjectValidator.ShouldNotBeNull();
            studentValidator.ShouldNotBeNull();
        }
        
        [Fact]
        public void Invoking_Add_On_Assembly_Registers_All_Implementations_With_Combined_Validators()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions))!;
            IServiceCollection services = _services.AddBusinessValidation(_assembly);
            IServiceProvider container = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = container.GetServices<IBusinessValidator<Subject>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));
            IBusinessValidator<Student> studentValidator = container.GetServices<IBusinessValidator<Student>>()
                .Single(t => t.GetType().Equals(typeof(CombinedValidator)));

            subjectValidator.ShouldNotBeNull();
            studentValidator.ShouldNotBeNull();
        }
    }
}
