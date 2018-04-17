using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Crif.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Serilog;

namespace Crif.Api
{
    [Route("[controller]")]
    public class CreditCheckController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICreditCheckService _crifService;
        public CreditCheckController(ILogger logger, ICreditCheckService crifService)
        {
            _logger = logger.ForContext<CreditCheckController>();
            _crifService = crifService;
        }
        public async Task<IActionResult> Post([FromBody]CreditCheckRequest orderCheckRequest)
        {
            var result = await _crifService.Check(orderCheckRequest.ToCreditCheckInquiry());

            return result.Match(
                 response => (IActionResult)Ok(response),
                 error =>  StatusCode(500));
        }

      
    }
}