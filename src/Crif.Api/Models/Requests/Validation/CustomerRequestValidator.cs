using System;
using FluentValidation;
using static Crif.Api.ValidationErrorCodes.CustomerRequest;

namespace Crif.Api
{
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerRequestValidator()
        {
            RuleFor(x => x.Phone)
                 .SetValidator(new PhoneRequestValidator()).When(x => x.Phone != null);

            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithErrorCode(LastNameRequired);
            
            RuleFor(x => x.FirstName)
                .NotEmpty()
                    .WithErrorCode(FirstNameRequired);

            RuleFor(x => x.Sex)
                .MustBeConvertibleToEnum(typeof(Sex)).When(x => !string.IsNullOrWhiteSpace(x.Sex))
                    .WithErrorCode(SexInvalid);

            RuleFor(x => x.Dob)
                .MustBeValidDate().When(x => !string.IsNullOrWhiteSpace(x.Dob))
                    .WithErrorCode(DobInvalid);

            RuleFor(x => x.Email)
                .MustBeValidEmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                    .WithErrorCode(EmailInvalid);
        }
    }
}