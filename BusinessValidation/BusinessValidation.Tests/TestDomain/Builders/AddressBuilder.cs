using BuilderGenerator;

namespace BusinessValidation.Tests.TestDomain.Builders
{
    [BuilderFor(typeof(Address))]
    public partial class AddressBuilder
    {
        public static AddressBuilder Simple()
        {
            return new AddressBuilder()
                .WithNumber(24)
                .WithStreet("Hancock")
                .WithSuburb("Palmerston")
                .WithState("Qld");
        }
    }
}
