using System;
using System.Collections.Generic;

namespace Crif.Api
{
    public class ErrorResponse
    {
        public ErrorResponse(string requestId, string errorType, IEnumerable<string> errorCodes)
        {
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException(nameof(requestId));
            if (string.IsNullOrEmpty(errorType)) throw new ArgumentNullException(nameof(errorType));

            RequestId = requestId;
            ErrorType = errorType;
            ErrorCodes = errorCodes;
        }

        public string RequestId { get; }
        public string ErrorType { get; }
        public IEnumerable<string> ErrorCodes { get; }
    }
}