using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Webhallen.Configurations;
using Webhallen.Models;

namespace Webhallen.Services
{
    public class WebhallenService : IWebhallenService
    {
        private readonly HttpClient _client;
        private readonly IWebhallenConfig _config;

        public WebhallenService(HttpClient client, IWebhallenConfig config)
        {
            _client = client;
            _config = config;
            client.BaseAddress = new Uri(_config.BaseUrl);
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/login", request, cancellationToken: ct);

            if (response.IsSuccessStatusCode == false)
                return null;

            string cookies = response.Headers
                .SingleOrDefault(header => header.Key == "Set-Cookie").Value
                .Aggregate((x, y ) => $"{x}\n{y}");

            const string last_visit = @"last_visit=([0-9]*)";
            const string webhallen_auth = @"\webhallen_auth=([a-zA-Z0-9\%_]*)";

            LoginResponse loginResponse = new LoginResponse(
                Regex.Match(cookies, last_visit).Groups[0].Value,
                Regex.Match(cookies, webhallen_auth).Groups[0].Value
            );

            _client.DefaultRequestHeaders.Add("Cookie", $"{loginResponse.WebhallenAuth};{loginResponse.LastVisit}");

            return loginResponse;
        }

        public async Task<SupplyDropResponse?> SupplyDropAsync(CancellationToken ct = default)
        {
            HttpRequestMessage message = new(HttpMethod.Get, $"api/supply-drop");
            HttpResponseMessage response = await _client.SendAsync(message, ct);
            string content = await response.Content.ReadAsStringAsync();
            SupplyDropResponse? output = JsonConvert.DeserializeObject<SupplyDropResponse>(content);
            return output;
        }

        public async Task<MeResponse?> MeAsync(CancellationToken ct = default)
        {
            HttpRequestMessage message = new(HttpMethod.Get, "api/me");
            HttpResponseMessage response = await _client.SendAsync(message, ct);
            string content = await response.Content.ReadAsStringAsync();
            MeResponse? output = JsonConvert.DeserializeObject<MeResponse>(content);
            return output;
        }
    }
}