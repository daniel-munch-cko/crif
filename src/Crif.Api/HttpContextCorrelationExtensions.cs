using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Crif.Api
{
    /// <summary>
    /// Correlation Extensions for HttpContext
    /// </summary>
    public static class HttpContextCorrelationExtensions
    {
        /// <summary>
        /// Gets the correlation identifier for the request
        /// </summary>
        /// <param name="httpContext">The HTTP Context</param>
        /// <returns>The Cko-Correlation-Id HTTP request header if present, otherwise <see cref="HttpContext.TraceIdentifier"/></returns>
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out var correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}