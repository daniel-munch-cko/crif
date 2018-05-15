using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crif.Api.Logging;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Crif.Api
{
    public class Startup
    {
        private readonly Serilog.ILogger _rootLogger;
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;


        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
            _rootLogger = Log.Logger;

            if (env.IsDevelopment())
            {
                _rootLogger.Debug(_configuration.Dump());
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOptions(services);
            AssertOptions();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));
            services.AddSingleton(_rootLogger);
            services.AddTransient<ICreditCheckService, CrifService>();
            services
                .AddMvcCore(options =>
                {
                    options.Filters.Add(new ValidateRequestFilter(new AttributedValidatorFactory()));
                    options.Filters.Add(new ValidateModelStateFilter());
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddJsonFormatters(serializerOptions =>
                {
                    // Snake casing by default
                    serializerOptions.NullValueHandling = NullValueHandling.Ignore;
                    serializerOptions.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });

            services.Configure<RouteOptions>(options =>
                options.LowercaseUrls = true);

            services
                .AddMetrics()
                .AddJsonSerialization()
                .AddHealthChecks()
                .AddMetricsMiddleware(config =>
                {
                    config.EnvironmentInfoEndpointEnabled = false;
                    config.MetricsEndpointEnabled = false;
                    config.MetricsTextEndpointEnabled = false;
                    config.ApdexTrackingEnabled = false;

                    config.PingEndpointEnabled = true;
                    config.HealthEndpointEnabled = true;
                    config.PingEndpoint = "/_system/ping";
                    config.HealthEndpoint = "/_system/health";
                });
        }

        public void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<CrifServiceOptions>(_configuration.GetSection("CrifService"));
        }

        private void AssertOptions()
        {
            var crifServiceOptions = new CrifServiceOptions();
            _configuration.GetSection("CrifService").Bind(crifServiceOptions);

            if (string.IsNullOrWhiteSpace(crifServiceOptions.Username))
            {
                throw new ArgumentException(nameof(crifServiceOptions.Url));
            }

            if (string.IsNullOrWhiteSpace(crifServiceOptions.Password))
            {
                throw new ArgumentException(nameof(crifServiceOptions.Password));
            }

            if (string.IsNullOrWhiteSpace(crifServiceOptions.Url))
            {
                throw new ArgumentException(nameof(crifServiceOptions.Url));
            }

            if (!Uri.TryCreate(crifServiceOptions.Url, UriKind.Absolute, out var _))
            {
                throw new ArgumentException(nameof(crifServiceOptions.Url));
            }


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(_rootLogger);

            // Since ASP.NET Core 2.0 no longer supports setting a base path using ASPNETCORE_URLS, 
            // we need to use the PathBase middleware (ref: https://github.com/aspnet/Announcements/issues/226)
            var pathBase = _configuration.GetValue<string>("PathBase");
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseCors("MyPolicy");
            app.UseMiddleware<ResponseHeadersMiddleware>();
            app.UseMiddleware<CorrelationIdLoggingMiddleware>(_rootLogger);
            app.UseMiddleware<RequestLoggingMiddleware>(_rootLogger);
            app.UseMetrics();
            app.UseMvc();
        }
    }
}
