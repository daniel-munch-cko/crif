using System.Threading.Tasks;
using OneOf;

namespace Crif.Api
{
    public interface ICreditCheckService
    {
        Task<OneOf<CreditCheckResult, CreditCheckErrorResponse>> Check(CreditCheckInquiry request);
    }
}