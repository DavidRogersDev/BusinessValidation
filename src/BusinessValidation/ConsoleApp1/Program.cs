using AthleteDomain;
using BusinessValidation;
using BusinessValidation.Extensions;
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
            Assembly domainAssembly = Assembly.GetAssembly(typeof(Athlete));

            services.AddBusinessValidation([assembly, domainAssembly]);
            services.AddScoped<Validator>(sp => new Validator());

            var sp = services.BuildServiceProvider();

            IBusinessValidator<Subject> subjectValidator = 
                sp.GetService<IBusinessValidator<Subject>>();
            var studentValidators = sp.GetServices<IBusinessValidator<Student>>();
            var athleteValidators = sp.GetServices<IBusinessValidator<Athlete>>();

            var localAthleteValidator = athleteValidators.First();
            var athleteValidator = athleteValidators.Skip(1).First();

            var results = studentValidators.Select(i => i.Validate(new Student()));
            var result2 = subjectValidator.Validate(new Subject());

            //foreach (var result in results)
            //{
            //    Console.WriteLine(result.IsValid);
            //    Console.WriteLine(result.ValidationFailures.Count);
            //}

            //new BusinessValidationResult()

            foreach (var result in results.Select(v => v.ValidationFailures))
            {
                foreach(var r in result)
                {
                    Console.WriteLine($"Key: {r.Key}");
                    foreach(var v in r.Value)
                    {
                        Console.WriteLine($"Failure: {v}");
                    }
                }
            }


            //Console.WriteLine(subjectValidator.Validator.ToString());
            //Console.WriteLine(result2.IsValid);

            var resultAthlete = athleteValidator.Validate(new Athlete { Id = 0, Team = Guid.Empty, Name = "Name" });
            var resultAthleteLocal = localAthleteValidator.Validate(new Athlete { Id = 0, Team = Guid.Empty, Name = "Name" });

            try
            {
                localAthleteValidator.Validator.ThrowIfInvalid();
            }
            catch (ValidationFailureException e)
            {

                Console.WriteLine(e.Failures);
            }
        }
    }
}
