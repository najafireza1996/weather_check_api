using System;
using WeatherProject.Domain.Entities;

namespace WeatherProject.Infrastructure.IRepositories
{
    public interface IExternalWeatherRepository
    {
        Task<Weather> FetchWeatherAsync();
    }
}

