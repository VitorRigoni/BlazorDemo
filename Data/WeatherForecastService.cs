using BlazorDemo.Options;
using LanguageExt;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace BlazorDemo.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly HttpClient _httpClient;
        private readonly ApplicationSettings _options;

        public WeatherForecastService(IHttpClientFactory httpClientFactory,
            IOptions<ApplicationSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _options = options.Value;
        }

        public Task<Option<OpenWeatherApiResult>> GetWeatherForecastForCityAsync(string city, string country)
        {
            Task<string> createUrl() =>
                $"{_options.WeatherApiUrl}/data/2.5/forecast/daily?q={city},{country}&cnt=7&appid={_options.WeatherApiId}"
                    .AsTask();

            Task<Validation<Exception, HttpResponseMessage>> callApi(string url) =>
                TryAsync(() => _httpClient.GetAsync(url))
                    .ToValidation(ex => ex);

            Task<Validation<Exception, string>> getContent(HttpResponseMessage response) =>
                TryAsync(() => response.Content.ReadAsStringAsync())
                    .ToValidation(ex => ex);

            Task<Validation<Exception, OpenWeatherApiResult>> parseJson(string stringData) =>
                Try(() => JsonConvert.DeserializeObject<OpenWeatherApiResult>(stringData))
                    .ToValidation()
                    .AsTask();

            return createUrl()
                .Bind(callApi)
                .BindT(getContent)
                .BindT(parseJson)
                .Map(x => x.ToOption());
        }

        public Task<WeatherForecastFakeData[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastFakeData
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
