using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Optional;

namespace Crif.Api
{
    public static class ModelExtensions
    {
        public static CreditCheckInquiry ToCreditCheckInquiry(this CreditCheckRequest request)
        {
            return new CreditCheckInquiry(request.Customer.ToCustomer(), request.BillingAddress.ToAddress());
        }

        public static CreditCheckResponse ToCreditCheckResponse(this CreditCheckResult result)
        {
            return new CreditCheckResponse { PaymentMethods = result.PaymentMethods.ToList() };
        }

        public static Phone ToPhone(this PhoneRequest phoneRequest)
        {
            return new Phone(phoneRequest.CountryCode, phoneRequest.Number);
        }

        public static Customer ToCustomer(this CustomerRequest customerRequest)
        {
            return new Customer(
                lastName: customerRequest.LastName,
                firstName: customerRequest.FirstName,
                maidenName: customerRequest.MaidenName.SomeWhenStringNonEmpty(),
                sex: customerRequest.Sex.ToOption<Sex>(),
                dateOfBirth: customerRequest.Dob.ToOption(),
                email: customerRequest.Email.SomeWhenStringNonEmpty(),
                phone: (customerRequest.Phone?.ToPhone()).SomeNotNull());
        }

        public static Address ToAddress(this AddressRequest addressRequest)
        {
            return new Address(
                street: addressRequest.Street,
                house: addressRequest.House,
                houseExtension: addressRequest.HouseExtension.SomeWhenStringNonEmpty(),
                city: addressRequest.City,
                zip: addressRequest.Zip,
                country: addressRequest.Country.ToUpper());
        }

        private static Option<T> ToOption<T>(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Option.None<T>();
            }
            return Option.Some(ToEnum<T>(value));
        }

        private static Option<DateTime> ToOption(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Option.None<DateTime>();
            }

            return Option.Some(DateTime.ParseExact(value, ValidationRules.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None));
        }
        private static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static Option<string> SomeWhenStringNonEmpty(this string value)
        {
            return value.SomeWhen(v => !string.IsNullOrWhiteSpace(v));
        }
    }
}