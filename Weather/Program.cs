
using Weather.Services;
using Weather.Services.Interfaces;

namespace Weather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Read config from appsettings.json
            var weatherApiBaseUrl = builder.Configuration["WeatherApi:BaseUrl"];


            // Register HttpClient for WeatherService
            builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri(weatherApiBaseUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
