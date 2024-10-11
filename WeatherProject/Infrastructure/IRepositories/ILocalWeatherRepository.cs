using System;
using WeatherProject.Domain.Entities;

namespace WeatherProject.Infrastructure.IRepositories
{
    public interface ILocalWeatherRepository : IRepository<Weather>
    {
        Task<Weather> GetLatestWeatherAsync();
    }
}

