using System;
using WeatherProject.Domain.Entities;

namespace WeatherProject.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherAsync();
    }
}

