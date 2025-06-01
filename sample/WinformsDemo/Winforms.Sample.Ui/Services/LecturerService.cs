using BusinessValidation;
using Winforms.Sample.Domain;
using Winforms.Sample.Domain.Validation;

namespace Winforms.Sample.Ui.Services
{
    public class LecturerService : ILecturerService
    {
        readonly IBusinessValidator<CreateLecturerValidationObject> createLecturerValidator;
        public LecturerService(IBusinessValidator<CreateLecturerValidationObject> createLecturerValidator)
        {
            this.createLecturerValidator = createLecturerValidator;
        }

        public bool CreateLecturer(Lecturer lecturer)
        {
            // obviously, this method would persist the object to some kind of data store

            CreateLecturerValidationObject validationObject = new() { Name = lecturer.Name };
            BusinessValidationResult result = createLecturerValidator.Validate(validationObject);

            if (result.NotValid) throw new ValidationFailureException(result.ValidationFailures);

            // if validation succeeds, proceed on to save the lecturer ...

            return true;
        }
    }
}
