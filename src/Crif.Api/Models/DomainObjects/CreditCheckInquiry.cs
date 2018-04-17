namespace Crif.Api
{
    public class CreditCheckInquiry
    {
        public Customer Customer { get; }
        public Address BillingAddress { get; }

        public CreditCheckInquiry(Customer customer, Address billingAddress)
        {
            Customer = customer;
            BillingAddress = billingAddress;
        }
    }
}