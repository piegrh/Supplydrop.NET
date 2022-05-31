using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using Webhallen.Services;
using NLog.Config;
using NLog.Extensions.Logging;
using Webhallen.Configurations;

namespace Webhallen
{
    public class Program
    {
        public class LoginOptions
        {
            [Option('u', "username", Required = true, HelpText = "Your username.")]
            public string? Username { get; set; }

            [Option('p', "password", Required = true, HelpText = "Your password.")]
            public string? Password { get; set; }
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string? user = Parser.Default.ParseArguments<LoginOptions>(args).Value.Username;
            string? pass = Parser.Default.ParseArguments<LoginOptions>(args).Value.Password;

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
                    services
                        .AddSingleton(Configuration)
                        .Configure<SupplyDropConfig>(o =>
                        {
                            o.Username = user ?? "";
                            o.Password = pass ?? "";
                        })
                        .Configure<WebhallenConfig>(Configuration.GetSection(nameof(WebhallenConfig)))
                        .AddSingleton<ISupplyDropConfig>(sp => sp.GetRequiredService<IOptions<SupplyDropConfig>>().Value)
                        .AddSingleton<IWebhallenConfig>(sp => sp.GetRequiredService<IOptions<WebhallenConfig>>().Value)
                        .AddHttpClient<WebhallenService>()
                        .ConfigurePrimaryHttpMessageHandler(x => new LoggingHandler(
                            Logger,
                            true,
                            new HttpClientHandler())
                        )
                        .Services
                        .AddTransient<IWebhallenService, WebhallenService>()
                        .AddSingleton<SupplyDropCollector>()
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
