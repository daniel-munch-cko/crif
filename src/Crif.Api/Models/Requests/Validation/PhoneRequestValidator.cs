using FluentValidation;
using static Crif.Api.ValidationErrorCodes.PhoneRequest;

namespace Crif.Api
{
    public class PhoneRequestValidator : AbstractValidator<PhoneRequest>
    {
        public PhoneRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty()
                    .WithErrorCode(CountryCodeRequired);

            RuleFor(x => x.Number)
                .NotEmpty()
                    .WithErrorCode(NumberRequired);
        }
    }
}