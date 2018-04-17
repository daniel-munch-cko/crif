using FluentValidation.Attributes;

namespace Crif.Api
{
    [Validator(typeof(CustomerRequestValidator))]
    public class CustomerRequest
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MaidenName { get; set; }
        public string Sex { get; set; }
        public string Dob { get; set; }
        public string Email { get; set; }
        public PhoneRequest Phone { get; set; }
    }
}