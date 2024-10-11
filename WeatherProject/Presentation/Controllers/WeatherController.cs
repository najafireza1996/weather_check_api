using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherProject.Application.Interfaces;


namespace WeatherProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

  
        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            var weather = await _weatherService.GetWeatherAsync();
            if (weather == null)
                return NoContent(); 

            return Ok(weather);
        }
    }
}

