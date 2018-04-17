using FluentValidation;

using static Crif.Api.ValidationErrorCodes.OrderCheckRequest;
namespace Crif.Api
{
    public class CreditCheckRequestValidator : AbstractValidator<CreditCheckRequest>
    {
        public CreditCheckRequestValidator()
        {
            RuleFor(x => x.Customer)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(CustomerRequired)
                .SetValidator(new CustomerRequestValidator());

            RuleFor(x => x.BillingAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                    .WithErrorCode(BillingAddressRequired)
                .SetValidator(new AddressRequestValidator());
        }
    }
}