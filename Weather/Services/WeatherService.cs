using System.Text.Json;
using Weather.Models;
using Weather.Services.Interfaces;

namespace Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        //public async Task<WeatherResponse?> GetCurrentWeatherAsync()
        //{
        //    string endpoint = _configuration["WeatherApi:Endpoint"];

        //    var response = await _httpClient.GetAsync(endpoint);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        _logger.LogError("Weather API failed with status code {StatusCode}", response.StatusCode);
        //        return null;
        //    }

        //    var json = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });
        //}

        public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
        {
            try
            {
                // 1️⃣ Get latitude/longitude from Geocoding API
                string? geoBaseUrl = _configuration["WeatherApi:GeocodingUrl"];
                using var geoClient = new HttpClient { BaseAddress = new Uri(geoBaseUrl) };

                var geoResponse = await geoClient.GetAsync($"/v1/search?name={city}&count=1");
                if (!geoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get geocoding info for {City}", city);
                    return null;
                }

                var geoJson = await geoResponse.Content.ReadAsStringAsync();
                var geoData = JsonSerializer.Deserialize<GeoResponse>(geoJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var location = geoData?.Results?.FirstOrDefault();
                if (location == null)
                {
                    _logger.LogWarning("City {City} not found", city);
                    return null;
                }

                double lat = location.Latitude;
                double lon = location.Longitude;

                // 2️⃣ Get weather using latitude/longitude
                string endpoint = $"/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true&timezone=auto";
                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Weather API failed for {City}", city);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching weather for {City}", city);
                return null;
            }
        }
    }
}
