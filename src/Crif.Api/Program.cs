using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crif.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            // Use ASPNETCORE Environment variables to set hosting URLs and base path e.g.
            // ASPNETCORE_URLS="http://*:5123"
            // ASPNETCORE_PATHBASE="/subpath"
            // Ref https://joonasw.net/view/aspnet-core-2-configuration-changes
            return new WebHostBuilder()
                .UseKestrel(x => x.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables(prefix: "CRIF_");

                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config.Build())
                        .Enrich.WithProperty("Version", ReflectionUtils.GetAssemblyVersion<Startup>())
                        .CreateLogger();
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
