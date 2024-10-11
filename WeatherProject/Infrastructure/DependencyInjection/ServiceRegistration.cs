using System;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherProject.Application.Interfaces;
using WeatherProject.Application.Services;
using WeatherProject.Infrastructure.Data;
using WeatherProject.Infrastructure.IRepositories;
using WeatherProject.Infrastructure.Repositories;
using WeatherProject.Infrastructure.Handlers;
using Polly;

namespace WeatherProject.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
        
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Repositories
            services.AddScoped<ILocalWeatherRepository, LocalWeatherRepository>();
            services.AddScoped<IExternalWeatherRepository, ExternalWeatherRepository>();

            services.AddHttpClient<IRequestHandler, RequestHandler>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //Services
            services.AddScoped<IWeatherService, WeatherService>();

            //Polly Policies
            services.AddPolicies();

            return services;
        }

        private static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            //Circuit Breaker Policy
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromMinutes(1));

            //Bulkhead Policy
            var bulkheadPolicy = Policy
                .BulkheadAsync(10, int.MaxValue); 

            //Timeout Policy
            var timeoutPolicy = Policy
                .TimeoutAsync(TimeSpan.FromSeconds(5));

            //DI Container
            services.AddSingleton(circuitBreakerPolicy);
            services.AddSingleton(bulkheadPolicy);
            services.AddSingleton(timeoutPolicy);

            return services;
        }
    }
}

