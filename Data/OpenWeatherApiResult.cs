using System;
using System.Collections.Generic;

namespace BlazorDemo.Data
{
    public class OpenWeatherApiResult
    {
        public OpenWeatherApiResultData List { get; set; }
    }

    public class OpenWeatherApiResultData
    {
        public DateTime Dt { get; set; }
        public ICollection<OpenWeatherApiResultTemperature> Temp { get; } = new List<OpenWeatherApiResultTemperature>();
        public ICollection<OpenWeatherApiResultWeather> Weather { get; } = new List<OpenWeatherApiResultWeather>();
    }

    public class OpenWeatherApiResultTemperature
    {
        public decimal Day { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Night { get; set; }
        public decimal Eve { get; set; }
        public decimal Morn { get; set; }
    }

    public class OpenWeatherApiResultWeather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
