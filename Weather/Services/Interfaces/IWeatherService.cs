using Weather.Models;

namespace Weather.Services.Interfaces
{
    public interface IWeatherService
    {
        //Task<WeatherResponse?> GetCurrentWeatherAsync();
        Task<WeatherResponse?> GetCurrentWeatherAsync(string city);
    }
}
