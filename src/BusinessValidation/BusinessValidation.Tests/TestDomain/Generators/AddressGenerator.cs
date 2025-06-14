using Bogus;

namespace BusinessValidation.Tests.TestDomain.Generators
{
    internal static class AddressGenerator
    {
        internal static Address GenerateSimple()
        {
            Faker<Address> faker = new Faker<Address>()
                .RuleForType(typeof(int), f => f.Random.Int())
                .RuleFor(p => p.PostCode, f => f.Random.Int(1, 5000))
                .RuleFor(p => p.State, f => "Qld")
                .RuleFor(p => p.Street, f => "Hancock")
                .RuleFor(p => p.Suburb, f => "Palmerston");

            return faker.Generate();
        }
    }
}
