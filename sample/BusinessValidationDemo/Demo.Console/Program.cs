using BusinessValidation;
using Demo.Console.Services;
using Demo.Console.Validators;
using Microsoft.Extensions.DependencyInjection;


var container = BuildContainer();

using var scope = container.CreateScope();

var enrollmentService = scope.ServiceProvider.GetService<IEnrollmentService>();

try
{
    var enrolled = enrollmentService.EnrolInUnit("ITN341", "n117832");

    if (enrolled)
        Console.WriteLine("Enrolled!!!");
}
catch (ValidationFailureException exception)
{
    DisplayFailures(exception);
}

Console.WriteLine();

try
{
    var consultationService = scope.ServiceProvider.GetService<IConsultationService>();

    var lecturer  = consultationService.GetLecturerForUnit("ITN341");

    // ... if valid, do something with lecturer object ...

}
catch (ValidationFailureException exception)
{
    DisplayFailures(exception);
}

Console.WriteLine();

// Demo adding raw validation messages
var externalValidation = new Validator();
externalValidation.AddFailure("ExternalError", "Some external validation failure happened");

var failures = externalValidation.ValidationFailures;
foreach (var failure in failures)
{
    Console.Write($"{failure.Key}: ");
    Console.Write(failure.Value.First());
}

Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}");


/******************************************************************************************************/

static IServiceProvider BuildContainer()
{
    var services = new ServiceCollection();
    services.AddSingleton<IEnrolmentAttemptValidator, EnrolmentAttemptValidator>();
    services.AddScoped<IEnrollmentService, EnrollmentService>();
    services.AddScoped<IConsultationService, ConsultationService>();

    return services.BuildServiceProvider();
}

static void DisplayFailures(ValidationFailureException exception)
{
    var validationFailures = exception.Failures;

    foreach (var failBundle in validationFailures)
    {
        Console.WriteLine(failBundle.Key);

        foreach (var validationFailure in failBundle.Value)
        {
            Console.WriteLine($"\t{validationFailure}");
        }
    }
}