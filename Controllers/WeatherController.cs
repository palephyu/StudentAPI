using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherController(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _apiKey = config["OpenWeather:ApiKey"];
            _baseUrl = config["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org/data/2.5";
        }

        // GET api/weather/{city}
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return Problem("OpenWeather API key not configured.");

            var client = _httpFactory.CreateClient();
            // units=metric to get Celsius
            var url = $"{_baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";

            HttpResponseMessage resp;
            try
            {
                resp = await client.GetAsync(url);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Error contacting weather service: {ex.Message}");
            }

            if (!resp.IsSuccessStatusCode)
            {
                var raw = await resp.Content.ReadAsStringAsync();
                return StatusCode((int)resp.StatusCode, raw);
            }

            var json = await resp.Content.ReadAsStringAsync();

            // Option A: Return raw OpenWeather JSON
            // return Content(json, "application/json");

            // Option B: Parse and return a small DTO (recommended)
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var result = new
            {
                City = root.GetProperty("name").GetString(),
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                FeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                Weather = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                Icon = root.GetProperty("weather")[0].GetProperty("icon").GetString()
            };

            return Ok(result);
        }
    }
}
