	using System;
namespace WeatherProject.Infrastructure.Handlers
{
    public class RequestHandler : IRequestHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RequestHandler> _logger;

        public RequestHandler(HttpClient httpClient, ILogger<RequestHandler> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"GET request to {url} failed with status code {response.StatusCode}.");
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GET request to {url} threw an exception.");
                return null;
            }
        }

        public async Task<string> PostAsync(string url, string content)
        {
            try
            {
                var httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"POST request to {url} failed with status code {response.StatusCode}.");
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"POST request to {url} threw an exception.");
                return null;
            }
        }
    }
}

