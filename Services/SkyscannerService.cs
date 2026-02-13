using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NamaTravelApi.Services;

public class SkyscannerService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public SkyscannerService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    // Calls Skyscanner Flights Indicative Prices endpoint and returns raw JSON (simplest for school project)
    public async Task<JsonDocument> IndicativeSearchAsync(string originIata, string destinationIata, DateOnly date)
    {
        var apiKey = _config["Skyscanner:ApiKey"];
        var baseUrl = _config["Skyscanner:BaseUrl"]?.TrimEnd('/');

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "PUT_YOUR_SKYSCANNER_KEY_HERE")
            throw new InvalidOperationException("Set Skyscanner:ApiKey in appsettings.json (or user-secrets).");

        var url = $"{baseUrl}/apiservices/v3/flights/indicative/search";

        // According to Skyscanner docs, the endpoint is POST /apiservices/v3/flights/indicative/search
        // with a JSON body containing market/locale/currency + queryLegs.  citeturn4view0
        var payload = new
        {
            query = new
            {
                market = "UK",
                locale = "en-GB",
                currency = "EUR",
                queryLegs = new object[]
                {
                    new {
                        originPlace = new { queryPlace = new { iata = originIata } },
                        destinationPlace = new { queryPlace = new { iata = destinationIata } },
                        fixedDate = new { year = date.Year, month = date.Month, day = date.Day }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Content = new StringContent(json, Encoding.UTF8, "application/json");

        // Auth header: Skyscanner docs use an API key (Partner auth). The exact header name depends on your account.
        // Most partners use: x-api-key: <key>
        req.Headers.Add("x-api-key", apiKey);

        using var resp = await _http.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var stream = await resp.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }
}
