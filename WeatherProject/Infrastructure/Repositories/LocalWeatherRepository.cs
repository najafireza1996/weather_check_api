using System;
using Microsoft.EntityFrameworkCore;
using WeatherProject.Domain.Entities;
using WeatherProject.Infrastructure.Data;
using WeatherProject.Infrastructure.IRepositories;

namespace WeatherProject.Infrastructure.Repositories
{
    public class LocalWeatherRepository : Repository<Weather>, ILocalWeatherRepository
    {
        public LocalWeatherRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Weather> GetLatestWeatherAsync()
        {
            return await dbSet.OrderByDescending(w => w.Time).FirstOrDefaultAsync();
        }
    }
}

