
using BuilderGenerator;

namespace BusinessValidation.Tests.TestDomain.Builders
{
    [BuilderFor(typeof(Lecturer))]
    public partial class LecturerBuilder
    {
        public const string LecturerFirstName = "Bob";
        public const string LecturerEmail = "bob.jones@uni.com";

        public static LecturerBuilder Simple()
        {
            var builder = new LecturerBuilder()
                .WithAge(50)
                .WithEmployeeNumber(16253)
                .WithFirstName(LecturerFirstName)
                .WithLastName("Jones")
                .WithEmailAddress(LecturerEmail)
                .WithCurrentlyRostered(true);

            return builder;
        }
    }
}
