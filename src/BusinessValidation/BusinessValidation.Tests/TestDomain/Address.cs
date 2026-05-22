namespace BusinessValidation.Tests.TestDomain
{
    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Suburb { get; set; } = string.Empty;
        public int PostCode { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
