using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Crif.Api.Logging
{
    /// <summary>
    /// Middleware that pushes correlation ID into the Serilog log context for the request
    /// </summary>
    public class CorrelationIdLoggingMiddleware
    {
        const string CorrelationProperty = "CorrelationId";

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CorrelationIdLoggingMiddleware(ILogger logger, RequestDelegate next)
        {           
            _logger = logger?.ForContext<CorrelationIdLoggingMiddleware>() ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
            
            using (LogContext.PushProperty(CorrelationProperty, httpContext.GetCorrelationId()))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}