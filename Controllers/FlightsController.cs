using Microsoft.AspNetCore.Mvc;
using NamaTravelApi.Services;

namespace NamaTravelApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public FlightsController(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights(string from, string to, string date)
    {
        var apiKey = _configuration["FlightAPI:ApiKey"];

        var url = $"https://api.flightapi.io/onewaytrip/{apiKey}/{from}/{to}/{date}/1/0/0/Economy";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return BadRequest("Error calling FlightAPI");

        var content = await response.Content.ReadAsStringAsync();

        return Ok(content);
    }
}