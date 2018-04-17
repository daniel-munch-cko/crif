using Crif.Api;
using FluentValidation.Attributes;

namespace Crif.Api
{
    [Validator(typeof(CreditCheckRequestValidator))]
    public class CreditCheckRequest
    {
        public CustomerRequest Customer { set; get; }
        public AddressRequest BillingAddress { set; get; }
    }
}