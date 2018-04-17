namespace Crif.Api
{
    public class ValidationErrorCodes
    {
        public class OrderCheckRequest
        {
            public const string CustomerRequired = "customer_required";
            public const string BillingAddressRequired = "billing_address_required";
        }

        public class CustomerRequest
        {
            public const string LastNameRequired = "customer_last_name_required";
            public const string FirstNameRequired = "customer_first_name_required";
            public const string SexInvalid = "customer_sex_invalid";
            public const string DobInvalid = "customer_dob_invalid";
            public const string EmailInvalid = "customer_email_invalid";
        }

        public class PhoneRequest
        {
            public const string CountryCodeRequired = "phone_country_code_requried";
            public const string NumberRequired = "phone_number_requried";
        }

        public class AddressRequest
        {
            public const string StreetRequired = "address_street_required";
            public const string HouseRequired = "address_house_required";
            public const string CityRequired = "address_city_required";
            public const string ZipRequired = "address_zip_required";
            public const string CountryRequired = "address_country_required";
            public const string CountryInvalid = "address_country_invalid";
        }
    }
}