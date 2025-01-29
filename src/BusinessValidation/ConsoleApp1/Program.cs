using BusinessValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Assembly assembly = Assembly.GetExecutingAssembly();

            services.AddBusinessValidation([assembly]);

            var sp = services.BuildServiceProvider();

            var studentValidator = sp.GetRequiredService<IBusinessValidator<StudentValidator>>();
            var subjectValidator = sp.GetRequiredService<IBusinessValidator<SubjectValidator>>();

            studentValidator.ExecuteValidation();            
            subjectValidator.ExecuteValidation();            

            Console.WriteLine(studentValidator.Validator.ToString());
            Console.WriteLine(subjectValidator.Validator.ToString());
        }
    }
}
