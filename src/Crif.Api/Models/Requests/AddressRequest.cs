using FluentValidation.Attributes;

namespace Crif.Api
{
    [Validator(typeof(AddressRequestValidator))]
    public class AddressRequest
    {
        public string Street { get; set; }
        public string House { get; set; }
        public string HouseExtension { set; get; }
        public string City { set; get; }
        public string Zip { set; get; }
        public string Country { set; get; }
    }
}