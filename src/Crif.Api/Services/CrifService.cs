using System.Threading.Tasks;
using System.ServiceModel;
using OneOf;
using System.ServiceModel.Channels;
using Crif.Api.Soap;
using Serilog;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace Crif.Api
{
    public class CrifService : ICreditCheckService
    {
        private readonly ILogger _logger;
        private readonly CrifServiceOptions _crifOptions;
        public CrifService(ILogger logger, IOptions<CrifServiceOptions> crifOptions)
        {
            _logger = logger.ForContext<CrifService>();
            _crifOptions = crifOptions.Value;
        }

        public async Task<OneOf<CreditCheckResult, CreditCheckErrorResponse>> Check(CreditCheckInquiry request)
        {
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            var factory = new ChannelFactory<OrderCheckPortType>(binding, new EndpointAddress(_crifOptions.Url));

            var serviceProxy = factory.CreateChannel();

            var soapMessageContext = new MessageContext
            {
                credentials = new Credentials
                {
                    user = _crifOptions.Username,
                    password = _crifOptions.Password
                }
            };

            var soapOrderCheckRequest = request.ToSoapOrderCheckRequest();
            var soapInput = new input
            {
                messageContext = soapMessageContext,
                orderCheckRequest = soapOrderCheckRequest
            };

            try
            {
                var result = await serviceProxy.orderCheckAsync(soapInput);
                var instructions = result
                                        ?.orderCheckResponse
                                        ?.clientExtensions
                                        ?.instruction
                                        ?.Where(i => i.type == "PAYMENT_METHOD")
                                        .Select(i => i.name)
                                        .ToList() ?? new List<string>();

                return new CreditCheckResult(instructions);
            }
            catch (FaultException<Crif.Api.Soap.Error> ex)
            {
                _logger.Error("CrifService returned an error code: {errorCode}, {errorMessage}", ex.Detail.code, ex.Detail.messageText);

                var errorCode = CreditCheckResultCodes.RemoteServiceError;

                if (ex.Detail.code == 400)
                {
                    errorCode = CreditCheckResultCodes.ValidationError;
                }
                if (ex.Detail.code == 401 || ex.Detail.code == 403)
                {
                    errorCode = CreditCheckResultCodes.ConfigurationError;
                }

                return new CreditCheckErrorResponse(errorCode, ex.Detail.messageText);
            }
            catch (EndpointNotFoundException)
            {   
                _logger.Error("CrifService Endpoint could not be found: {Url}", _crifOptions.Url);
                return new CreditCheckErrorResponse(CreditCheckResultCodes.ConfigurationError, "");
            }
        }
    }
}