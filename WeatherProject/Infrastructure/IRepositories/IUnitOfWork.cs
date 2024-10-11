using System;
namespace WeatherProject.Infrastructure.IRepositories
{
    public interface IUnitOfWork
    {
        ILocalWeatherRepository WeatherRepo { get; }
        Task SaveAsync();
    }
}

