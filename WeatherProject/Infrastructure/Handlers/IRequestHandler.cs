using System;
namespace WeatherProject.Infrastructure.Handlers
{
    public interface IRequestHandler
    {
        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, string content);
    }
}

