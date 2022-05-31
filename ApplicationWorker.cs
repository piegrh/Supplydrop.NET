using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Webhallen.Services;

namespace Webhallen
{
    public class ApplicationWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly SupplyDropCollector _supplyDropCollector;
        private readonly ILogger<ApplicationWorker> _logger;

        public ApplicationWorker(
            IHostApplicationLifetime applicationLifetime, 
            SupplyDropCollector supplyDropCollector, 
            ILogger<ApplicationWorker> logger)
        {
            _applicationLifetime = applicationLifetime;
            _supplyDropCollector = supplyDropCollector;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await _supplyDropCollector.CollectSupplyDropAsync();
                stopwatch.Stop();
                _logger.LogInformation($"ApplicationWorker finished after {stopwatch.ElapsedMilliseconds} ms.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error running application!");
            }
            finally
            {
                _applicationLifetime.StopApplication();
                _logger.LogInformation("Done.");
            }
        }
    }
}
