using BusinessValidation;
using Demo.Console.Domain;
using System.Diagnostics;


namespace Demo.Console.Services
{
    /// <summary>
    /// In this sample service, validation is externalised to a separate validator class which is injected via its abstraction.
    /// </summary>
    public class ConsultationService : IConsultationService
    {
        public Lecturer GetLecturerForUnit(string unitNumber)
        {
            var validator = new Validator();

            var lecturer = GetLecturerBySubject(unitNumber);

            validator.Validate(l => l.CurrentlyRostered, $"{lecturer.FirstName} {lecturer.LastName} is not currently available.", lecturer, l => l.CurrentlyRostered);
            validator.Validate(l => l.Address.PostCode, $"{lecturer.Address.PostCode} is not a close enough postcode for consultations.", lecturer, l => l.Address.PostCode > 1999 & l.Address.PostCode < 3000);

            validator.ThrowIfInvalid();

            return lecturer; 
        }

        private Lecturer GetLecturerBySubject(string unitNumber)
        {
            // fake a db call and return the lecturer.
            return new Lecturer
            {
                FirstName = "Petro",
                LastName = "Jones",
                CurrentlyRostered = false,
                Address = new Address
                {
                    Number = 2,
                    State = "New North Zeal",
                    Street = "Floral Blvd",
                    PostCode = 4000,
                    Suburb = "Doberman Palisades"
                }
            };
        }
    }
}
