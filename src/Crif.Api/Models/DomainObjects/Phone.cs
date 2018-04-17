namespace Crif.Api
{
    public class Phone
    {
        public string CountryCode { get; }
        public string Number { get; }

        public Phone(string countryCode, string number)
        {
            CountryCode = countryCode;
            Number = number;
        }
    }
}