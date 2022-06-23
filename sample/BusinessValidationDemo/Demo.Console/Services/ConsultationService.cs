using BusinessValidation;
using Demo.Console.Domain;
using System.Diagnostics;


namespace Demo.Console.Services
{
    public class ConsultationService : IConsultationService
    {
        public Lecturer GetLecturerForUnit(string unitNumber)
        {
            var validator = new Validator();

            var lecturer = GetLecturerBySubject(unitNumber);

            validator.Validate(l => l.CurrentlyRostered, $"{lecturer.FirstName} {lecturer.LastName} is not currently available.", lecturer, l => l.CurrentlyRostered);
            validator.Validate(l => l.Address.Number, $"{lecturer.Address.PostCode} is not a valid '{{fail-bundle}}'.", lecturer, l => l.Address.PostCode > 1999 & l.Address.PostCode < 3000);

            Trace.WriteLine(validator);

            validator.Throw();

            return null;
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
                    PostCode = 2673,
                    Suburb = "Doberman Palisades"
                }
            };
        }
    }
}
