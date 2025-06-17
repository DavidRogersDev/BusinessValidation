using Bogus;

namespace BusinessValidation.Tests.TestDomain.Generators
{
    internal static class LecturerGenerator
    {
        internal static Lecturer GenerateSimple()
        {
            Faker<Lecturer> faker = new Faker<Lecturer>()
                .RuleForType(typeof(int), f => f.Random.Int())
                .RuleFor(p => p.EmailAddress, f => "bob.jones@uni.com")
                .RuleFor(p => p.FirstName, f => "bob")
                .RuleFor(p => p.LastName, f => "Jones")
                .RuleFor(p => p.CurrentlyRostered, f => true);

            return faker.Generate();
        }
        
        internal static Lecturer GenerateSimpleWithAddress()
        {
            Faker<Lecturer> faker = new Faker<Lecturer>()
                .RuleForType(typeof(int), f => f.Random.Int())
                .RuleFor(p => p.Address, f => AddressGenerator.GenerateSimple())
                .RuleFor(p => p.EmailAddress, f => "bob.jones@uni.com")
                .RuleFor(p => p.FirstName, f => "bob")
                .RuleFor(p => p.LastName, f => "Jones")
                .RuleFor(p => p.CurrentlyRostered, f => true);

            return faker.Generate();
        }
    }
}
