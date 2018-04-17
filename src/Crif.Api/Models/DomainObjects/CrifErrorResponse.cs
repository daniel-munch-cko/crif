namespace Crif.Api
{
    public class CreditCheckErrorResponse
    {
        public readonly CreditCheckResultCodes ResultCode;
        public readonly string Message;

        public CreditCheckErrorResponse(CreditCheckResultCodes resultCode, string message)
        {
            ResultCode = resultCode;
            Message = message;
        }
    }
}