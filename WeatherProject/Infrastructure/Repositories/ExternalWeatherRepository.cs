using System;
using Newtonsoft.Json.Linq;
using WeatherProject.Domain.Entities;
using WeatherProject.Infrastructure.Handlers;
using WeatherProject.Infrastructure.IRepositories;

namespace WeatherProject.Infrastructure.Repositories
{
    public class ExternalWeatherRepository : IExternalWeatherRepository
    {
        private readonly IRequestHandler _requestHandler;
        private readonly ILogger<ExternalWeatherRepository> _logger;

        public ExternalWeatherRepository(IRequestHandler requestHandler, ILogger<ExternalWeatherRepository> logger)
        {
            _requestHandler = requestHandler;
            _logger = logger;
        }

        public async Task<Weather> FetchWeatherAsync()
        {
            var url = "https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m";
            try
            {
                var response = await _requestHandler.GetAsync(url);
                if (string.IsNullOrEmpty(response))
                {
                    _logger.LogWarning("External API returned null or empty response.");
                    return null;
                }

                var json = JObject.Parse(response);
                var times = json["hourly"]["time"].ToObject<string[]>();
                var temperatures = json["hourly"]["temperature_2m"].ToObject<double[]>();

                if (times == null || temperatures == null || times.Length == 0 || temperatures.Length == 0)
                {
                    _logger.LogWarning("External API response does not contain expected data.");
                    return null;
                }

                var latestIndex = times.Length - 1;
                return new Weather
                {
                    Time = DateTime.Parse(times[latestIndex]),
                    Temperature2m = temperatures[latestIndex]
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data from external API.");
                return null;
            }
        }
    }
}

