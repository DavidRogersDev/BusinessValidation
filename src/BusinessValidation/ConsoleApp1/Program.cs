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

            IBusinessValidator<Subject> subjectValidator = sp.GetRequiredService<IBusinessValidator<Subject>>();
            var studentValidators = sp.GetServices<IBusinessValidator<Student>>();

            var results = studentValidators.Select(i => i.Validate(new Student()));
            var result2 = subjectValidator.Validate(new Subject());

            foreach (var result in results)
            {
                Console.WriteLine(result.IsValid);
                Console.WriteLine(result.ValidationFailures.Count);
            }

            //foreach (var studentValidator in studentValidators)
            //{
            //    Console.WriteLine(studentValidator.Validator.ToString());                
            //}


            Console.WriteLine(subjectValidator.Validator.ToString());
            Console.WriteLine(result2.IsValid);
        }
    }
}
