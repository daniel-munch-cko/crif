using FluentValidation;

using static Crif.Api.ValidationErrorCodes.AddressRequest;
namespace Crif.Api
{
    public class AddressRequestValidator : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                    .WithErrorCode(StreetRequired);

            RuleFor(x => x.House)
                .NotEmpty()
                    .WithErrorCode(HouseRequired);

            RuleFor(x => x.City)
               .NotEmpty()
                   .WithErrorCode(CityRequired);

            RuleFor(x => x.Zip)
               .NotEmpty()
                   .WithErrorCode(ZipRequired);

            RuleFor(x => x.Country)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
                   .WithErrorCode(CountryRequired)
               .MustBeValidCountryCode()
                    .WithErrorCode(CountryInvalid);
        }
    }
}