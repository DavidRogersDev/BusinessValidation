
using BuilderGenerator;

namespace BusinessValidation.Tests.TestDomain.Builders
{
    [BuilderFor(typeof(Lecturer))]
    public partial class LecturerBuilder
    {
        public const string LecturerFirstName = "Bob";
        public const string LecturerEmail = "bob.jones@uni.com";
        public const string LecturerSurname = "Jones";
        public const int LecturerEmployeeNumber = 16253;
        public const bool RosteredOn = true;

        public static LecturerBuilder Simple()
        {
            var builder = new LecturerBuilder()
                .WithAge(50)
                .WithEmployeeNumber(LecturerEmployeeNumber)
                .WithFirstName(LecturerFirstName)
                .WithLastName(LecturerSurname)
                .WithEmailAddress(LecturerEmail)
                .WithCurrentlyRostered(RosteredOn);

            return builder;
        }
        
        public static LecturerBuilder SimpleWithAddress()
        {
            var builder = new LecturerBuilder()
                .WithAddress(AddressBuilder.Simple().Build())
                .WithAge(50)
                .WithEmployeeNumber(16253)
                .WithFirstName(LecturerFirstName)
                .WithLastName(LecturerSurname)
                .WithEmailAddress(LecturerEmail)
                .WithCurrentlyRostered(true);

            return builder;
        }
    }
}
