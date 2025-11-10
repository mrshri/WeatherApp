using Microsoft.AspNetCore.Mvc;
using Weather.Services.Interfaces;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        //[HttpGet("current")]
        //public async Task<IActionResult> GetCurrentWeather()
        //{
        //    var weather = await _weatherService.GetCurrentWeatherAsync();
        //    if (weather == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(weather);
        //}
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var result = await _weatherService.GetCurrentWeatherAsync(city);
            if (result == null)
                return NotFound($"Weather data not found for city: {city}");

            return Ok(result);
        }
    }
}
