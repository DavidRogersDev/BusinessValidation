using BusinessValidation;
using Microsoft.Extensions.DependencyInjection;
using Winforms.Sample.Ui.Services;
using System.Text;
using Winforms.Sample.Domain;

namespace Winforms.Sample.Ui
{
    public partial class Main : Form
    {
        readonly IServiceProvider serviceProvider;

        public Main(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            InitializeComponent();

            TextBoxInstructionsLecturer.Text = $"To violate a validation rule, submit a lecturer name that is 9 characters or longer.{Environment.NewLine}To violate a second rule, enter the name 'Beelzebub'.";
            TextBoxInstructionsUnit.Text = $"To violate a validation rule, submit a unit name that is 11 characters or longer.{Environment.NewLine}To violate a second rule, enter the name 'Alchemy 101'.{Environment.NewLine}To violate a third rule, enter an Id in the second textbox which is 0 (or just leave it empty).";
        }

        private void ButtonSubmitLecturer_Click(object sender, EventArgs e)
        {
            PanelErrorList.Visible = false;
            LabelErrors.Text = string.Empty;

            try
            {
                // Use a service locator because we want a fresh instance of the LecturerService,
                // with the injected validator, with every button-click. Otherwise, state will
                // be maintained between button clicks and we don't want that.
                ILecturerService lecturerService = serviceProvider.GetService<ILecturerService>()!;

                lecturerService.CreateLecturer(new Lecturer
                {
                    Name = TextBoxName.Text.Trim(),
                    Id = Guid.NewGuid()
                });
            }
            catch (ValidationFailureException exception)
            {

                StringBuilder sb = new StringBuilder();

                _ = exception.Failures.SelectMany(f => f.Value.Select(vf =>
                {
                    sb.AppendLine(vf);
                    return vf;
                })).ToList();

                LabelErrors.Text = sb.ToString();

                PanelErrorList.Visible = true;

            }
        }

        private void ButtonSubmitUnit_Click(object sender, EventArgs e)
        {
            PanelErrorList2.Visible = false;
            LabelErrors2.Text = string.Empty;

            try
            {
                // Use a service locator because we want a fresh instance of the LecturerService,
                // with the injected validator, with every button-click. Otherwise, state will
                // be maintained between button clicks and we don't want that.
                IUnitService unitService = serviceProvider.GetService<IUnitService>()!;

                unitService.CreateUnit(new Unit
                {
                    Name = TextBoxUnit.Text.Trim(),

                    // we are not validating this in the UI. That is not the role of BusinessValidation. But you should guard against a failure to parse an int here.
                    Id = string.IsNullOrWhiteSpace(TextBoxUnitId.Text)
                        ? 0
                        : int.Parse(TextBoxUnitId.Text),
                });
            }
            catch (ValidationFailureException exception)
            {

                StringBuilder sb = new StringBuilder();

                _ = exception.Failures.SelectMany(f => f.Value.Select(vf =>
                {
                    sb.AppendLine("❌ - " + vf);
                    return vf;
                })).ToList();

                LabelErrors2.Text = sb.ToString();

                PanelErrorList2.Visible = true;
            }
        }
    }
}
