using Optional;

namespace Crif.Api
{
    public class Address
    {
        public string Street { get; }
        public string House { get; }
        public Option<string> HouseExtension { get; }
        public string City { get; }
        public string Zip { get; }
        public string Country { get; }

        public Address(
            string street,
            string house,
            Option<string> houseExtension,
            string city,
            string zip,
            string country)
        {
            Street = street;
            House = house;
            HouseExtension = houseExtension;
            City = city;
            Zip = zip;
            Country = country;
        }
    }
}