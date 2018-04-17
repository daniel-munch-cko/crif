using System.Collections.Generic;

namespace Crif.Api
{
    public class CreditCheckResult
    {
        public IReadOnlyList<string> PaymentMethods { get; }

        public CreditCheckResult(IReadOnlyList<string> paymentMethods)
        {
            PaymentMethods = paymentMethods;
        }
    }
}