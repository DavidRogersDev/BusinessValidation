using BuilderGenerator;

namespace BusinessValidation.Tests.TestDomain.Builders
{
    [BuilderFor(typeof(Address))]
    public partial class AddressBuilder
    {
        public const string StreetName = "Hancock";
        public const int StreetNumber = 24;
        public const int PostCodeValue = 4000;
        public const string SuburbName = "Palmerston";
        public const string StateName = "Qld";

        public static AddressBuilder Simple() => new AddressBuilder()
                .WithNumber(StreetNumber)
                .WithStreet(StreetName)
                .WithSuburb(SuburbName)
                .WithPostCode(PostCodeValue)
                .WithState(StateName);
    }
}
