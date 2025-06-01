using BusinessValidation.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Winforms.Sample.Ui.Services;
using Winforms.Sample.Ui.Validation;
using Winforms.Sample.Domain.Validation;

namespace Winforms.Sample.Ui
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IServiceCollection services = new ServiceCollection();
            services.AddBusinessValidation([typeof(UnitNameValidator).Assembly, typeof(CreateLecturerValidator).Assembly], ServiceLifetime.Transient);
            services.AddTransient<ILecturerService, LecturerService>();
            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<Main>();

            var container = services.BuildServiceProvider();

            var mainForm = container.GetRequiredService<Main>();

            Application.Run(mainForm);
        }
    }
}