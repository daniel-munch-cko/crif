using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Crif.Api
{
    public class ResponseHeadersMiddleware
    {
        private static Lazy<string> Version = new Lazy<string>(() =>
            ReflectionUtils.GetAssemblyVersion<ResponseHeadersMiddleware>());

        private readonly RequestDelegate _next;

        public ResponseHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("Cko-Request-Id", context.TraceIdentifier);
                context.Response.Headers.Add("Cko-Version", Version.Value);
                return Task.CompletedTask;
            });

            return _next.Invoke(context);
        }
    }
}