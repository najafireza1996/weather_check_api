using System;
using WeatherProject.Infrastructure.Data;
using WeatherProject.Infrastructure.IRepositories;

namespace WeatherProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public ILocalWeatherRepository WeatherRepo { get; }

        public UnitOfWork(AppDbContext dbContext, ILocalWeatherRepository localWeatherRepository)
        {
            _dbContext = dbContext;
            WeatherRepo = localWeatherRepository;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

