using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using Webhallen.Configurations;
using Webhallen.Extensions;
using Webhallen.Models;

namespace Webhallen.Services
{
    public class SupplyDropCollector
    {
        private readonly IWebhallenService _service;
        private readonly ILogger<SupplyDropCollector> _logger;
        private readonly ISupplyDropConfig _configuration;

        public SupplyDropCollector(IWebhallenService service, ILogger<SupplyDropCollector> logger, ISupplyDropConfig configuration)
        {
            _service = service;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task CollectSupplyDropAsync()
        {
            _logger.LogInformation("LoginAsync...");
            var loginResponse = await GetLoginResponseAsync();

            if (loginResponse is null)
            {
                _logger.LogError("LoginAsync failed! Wrong username/password.");
                return;
            }

            var previousDrops = await GetDropsAsync();
            var dropsBeforeSum = previousDrops.Select(x => x.count).Sum();

            _logger.LogDebug($"Current supply drops collected: {dropsBeforeSum}");

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            _logger.LogInformation("Navigating to webhallen...");
            await page.GoToAsync("https://www.webhallen.com");
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            });

            _logger.LogDebug("Set cookies...");
            await SetCookieAsync(loginResponse?.WebhallenAuth, page);
            await SetCookieAsync(loginResponse?.LastVisit, page);

            await Task.Delay(100);

            long userId = await GetUserIdAsync();

            _logger.LogInformation("Navigating to supply-drop page...");
            await page.GoToAsync($"https://www.webhallen.com/se/member/{userId}/supply-drop");

            for (int i = 0; i < 3; i++)
            {
                _logger.LogInformation($"Clicking on supply drop. ({i + 1}/3)");
                await Task.Delay(5_000);
                await page.ClickAsync(_configuration.SupplyDropSelector);
            }

            List<Drop> currentDrops = await GetDropsAsync();
            IEnumerable<Drop> dropDiff = GetDropDifference(previousDrops, currentDrops);

            if (dropDiff.Any() == false)
            {
                _logger.LogInformation("No supply drop found!");
                return;
            }

            _logger.LogInformation("Collected: ");
            foreach (Drop drop in dropDiff)
                _logger.LogInformation(drop.ToString());
        }

        private static async Task SetCookieAsync(string? cookie, Page page)
        {
            string[] keyValue = cookie.Split("=") ?? new[] {"", ""};
            await page.SetCookieAsync(new CookieParam
            {
                Name = keyValue[0],
                Value = keyValue[1],
            });
        }

        private async Task<LoginResponse?> GetLoginResponseAsync()
        {
            LoginRequest request = new(_configuration.Username, _configuration.Password);
            LoginResponse? loginResponse = await _service.LoginAsync(request);
            return loginResponse;
        }

        private async Task<long> GetUserIdAsync()
        {
            MeResponse? meResponse = await _service.MeAsync();
            return meResponse?.User.Id ?? -1;
        }

        private async Task<List<Drop>> GetDropsAsync()
        {
           var response = await _service.SupplyDropAsync();
           return response?.drops ?? new List<Drop>();
        }

        public static IEnumerable<Drop> GetDropDifference(IEnumerable<Drop> previousDrops, IEnumerable<Drop> currentDrops)
        {
            Dictionary<int, Drop[]> previous = previousDrops.ToLookUpDictionary(x => x.item.id);
            Dictionary<int, Drop[]> current = currentDrops.ToLookUpDictionary(x => x.item.id);
            List<Drop> difference = new ();
            foreach (KeyValuePair<int, Drop[]> kp in current)
            {
                if (previous.ContainsKey(kp.Key) && kp.Value.First().count == previous[kp.Key].First().count)
                    continue;
                difference.Add(new Drop()
                {
                    item = kp.Value.First().item,
                    count = kp.Value.First().count - (previous.ContainsKey(kp.Key) ? previous[kp.Key].First().count : 0)
                });
            }
            return difference;
        }
    }
}
