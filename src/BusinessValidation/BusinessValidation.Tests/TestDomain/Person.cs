namespace BusinessValidation.Tests.TestDomain
{
    public class Person
    {
        public Address? Address { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
