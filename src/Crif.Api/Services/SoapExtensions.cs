using System;
using System.Collections.Generic;
using System.Linq;

namespace Crif.Api
{
    public static class SoapExtensions
    {
        public static Soap.OrderCheckRequest ToSoapOrderCheckRequest(this CreditCheckInquiry inquiry)
        {
            var product = new Soap.Product
            {
                name = CrifConfiguration.Name,
                country = CrifConfiguration.Country,
                proofOfInterest = CrifConfiguration.Poi
            };

            var soapRequest = new Soap.OrderCheckRequest
            {
                product = product,
                searchedAddress = inquiry.ToSoapSearchedAddress(),
            };

            return soapRequest;
        }

        public static Soap.SearchedAddress ToSoapSearchedAddress(this CreditCheckInquiry inquiry)
        {
            var contact = new List<Soap.Contact>();

            inquiry.Customer.Phone.MatchSome(val =>
            {
                contact.Add(new Soap.Contact { item = "PHONE", value = $"{val.CountryCode}{val.Number}" });
            });

            inquiry.Customer.Email.MatchSome(val =>
            {
                contact.Add(new Soap.Contact { item = "EMAIL", value = val });
            });

            var soapSearchedAddress = new Soap.SearchedAddress
            {
                legalForm = Soap.LegalForm.PERSON,
                legalFormSpecified = true,
                address = inquiry.ToSoapAddress()
            };

            if (contact.Count > 0)
            {
                soapSearchedAddress.contact = contact.ToArray();
            }
            return soapSearchedAddress;
        }

        public static Soap.Address ToSoapAddress(this CreditCheckInquiry inquiry)
        {
            var soapAddress = new Soap.Address
            {
                name = inquiry.Customer.LastName,
                firstName = inquiry.Customer.FirstName
            };

            inquiry.Customer.MaidenName.MatchSome(val => soapAddress.maidenName = val);
            inquiry.Customer.Sex.MatchSome(val =>
            {
                soapAddress.gender = val.ToSoapGender();
                soapAddress.genderSpecified = true;
            });

            inquiry.Customer.DateOfBirth.MatchSome(val =>
            {
                soapAddress.dateOfBirth = int.Parse(val.ToString("yyyyMMdd"));
                soapAddress.dateOfBirthSpecified = true;
            });

            soapAddress.location = new Soap.Location
            {
                street = inquiry.BillingAddress.Street,
                house = inquiry.BillingAddress.House,
                city = inquiry.BillingAddress.City,
                zip = inquiry.BillingAddress.Zip,
                country = inquiry.BillingAddress.Country
            };

            inquiry.BillingAddress.HouseExtension.MatchSome(val => soapAddress.location.houseExtension = val);

            return soapAddress;
        }

        public static Soap.Contact ToSoapContact(this KeyValuePair<string, string> keyValuePair)
        {
            return new Soap.Contact
            {
                item = keyValuePair.Key,
                value = keyValuePair.Value
            };
        }
        public static Soap.Gender ToSoapGender(this Sex gender)
        {
            switch (gender)
            {
                case Sex.Male:
                    return Soap.Gender.MALE;
                case Sex.Female:
                    return Soap.Gender.FEMALE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }
        }

        public static Soap.LegalForm ToSoapLegalForm(this LegalForm legalForm)
        {
            switch (legalForm)
            {
                case LegalForm.Unknown:
                    return Soap.LegalForm.UNKNOWN;
                case LegalForm.Person:
                    return Soap.LegalForm.PERSON;
                case LegalForm.Company:
                    return Soap.LegalForm.COMPANY;
                default:
                    throw new ArgumentOutOfRangeException(nameof(legalForm));
            }
        }
    }
}