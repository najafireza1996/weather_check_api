using System;
namespace WeatherProject.Domain.Entities
{
    public class Weather
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Temperature2m { get; set; }
    }
}
