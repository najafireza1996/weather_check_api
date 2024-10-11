using System;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Timeout;
using WeatherProject.Application.Interfaces;
using WeatherProject.Domain.Entities;
using WeatherProject.Infrastructure.IRepositories;

namespace WeatherProject.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalWeatherRepository _externalWeatherRepository;
        private readonly ILogger<WeatherService> _logger;

        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly AsyncBulkheadPolicy _bulkheadPolicy;
        private readonly AsyncTimeoutPolicy _timeoutPolicy;

        public WeatherService(
            IUnitOfWork unitOfWork,
            IExternalWeatherRepository externalWeatherRepository,
            ILogger<WeatherService> logger,
            AsyncCircuitBreakerPolicy circuitBreakerPolicy,
            AsyncBulkheadPolicy bulkheadPolicy,
            AsyncTimeoutPolicy timeoutPolicy)
        {
            _unitOfWork = unitOfWork;
            _externalWeatherRepository = externalWeatherRepository;
            _logger = logger;

            _circuitBreakerPolicy = circuitBreakerPolicy;
            _bulkheadPolicy = bulkheadPolicy;
            _timeoutPolicy = timeoutPolicy;
        }

        public async Task<Weather> GetWeatherAsync()
        {
            try
            {
                return await _circuitBreakerPolicy
                    .WrapAsync(_bulkheadPolicy)
                    .WrapAsync(_timeoutPolicy)
                    .ExecuteAsync(async () =>
                    {
                        var weather = await _externalWeatherRepository.FetchWeatherAsync();
                        if (weather != null)
                        {
                            await _unitOfWork.WeatherRepo.AddAsync(weather);
                            await _unitOfWork.SaveAsync();
                            return weather;
                        }
                        else
                        {
                            var lastWeather = await _unitOfWork.WeatherRepo.GetLatestWeatherAsync();
                            return lastWeather;
                        }
                    });
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogWarning(ex, "Circuit breaker is open. Returning last known weather data.");
                var lastWeather = await _unitOfWork.WeatherRepo.GetLatestWeatherAsync();
                return lastWeather;
            }
            catch (TimeoutRejectedException ex)
            {
                _logger.LogWarning(ex, "Request timed out. Returning last known weather data.");
                var lastWeather = await _unitOfWork.WeatherRepo.GetLatestWeatherAsync();
                return lastWeather;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetWeatherAsync.");
                return null;
            }
        }
    }
}

