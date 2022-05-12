using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Webhallen.Services;
using NLog.Config;
using NLog.Extensions.Logging;

namespace Webhallen
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(config =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables(prefix: "DOTNET_");
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables(prefix: "DOTNET_");
                    Configuration = config.Build();
                    ConfigureLogging();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging
                        .ClearProviders()
                        .AddNLog();
                })
                .UseDefaultServiceProvider((context, options) =>
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .ConfigureServices((context, services) =>
                {
                    if (Configuration == null)
                        throw new Exception("NO CONFIG LOADED");
                    WebhallenConfiguration config = new ();
                    Configuration.Bind("Webhallen", config);
                    services
                        .AddSingleton(config)
                        .AddTransient<WebhallenService>()
                        .AddHttpClient<WebhallenService>(x => x.BaseAddress = new Uri(config.BaseUrl))
                            .ConfigurePrimaryHttpMessageHandler(x => new LoggingHandler(
                                Logger ,
                                true, 
                                new HttpClientHandler())
                            )
                        .Services
                        .AddTransient<SupplyDropCollector>()
                        .AddHostedService<ApplicationWorker>();

                })
                .UseConsoleLifetime();
        }

        private static void ConfigureLogging()
        {
            XmlLoggingConfiguration nlogConfig = new (string.Concat(Directory.GetCurrentDirectory(), $"/Log/nlog.config"));
            LogManager.Configuration = nlogConfig;
        }

        public static IConfigurationRoot? Configuration { get; set; }

        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}
