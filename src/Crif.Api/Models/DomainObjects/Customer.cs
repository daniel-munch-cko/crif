using System;
using Optional;

namespace Crif.Api
{
    public class Customer
    {
        public string LastName { get; }
        public string FirstName { get; }
        public Option<string> MaidenName { get; }
        public Option<Sex> Sex { get; }
        public Option<DateTime> DateOfBirth { get; }
        public Option<string> Email { get; }
        public Option<Phone> Phone { get; }

        public Customer(
                string lastName,
                string firstName,
                Option<string> maidenName,
                Option<Sex> sex,
                Option<DateTime> dateOfBirth,
                Option<string> email,
                Option<Phone> phone)
        {
            LastName = lastName;
            FirstName = firstName;
            MaidenName = maidenName;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Email = email;
            Phone = phone;
        }
    }
}