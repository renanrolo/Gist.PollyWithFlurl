using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gist.PollyWithFlurl.Api.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("WeatherForecast")]
        public string Index()
        {
            return "Hello World!";
        }


        [HttpGet]
        [Route("WeatherForecast/Success")]
        public IEnumerable<WeatherForecast> Success()
        {
            return ReturnSuccess();
        }

        [HttpGet]
        [Route("WeatherForecast/SuccessOnThirdTry")]
        public IEnumerable<WeatherForecast> SuccessOnThirdTry()
        {

            if (Singleton.GetSession.SuccessOnThirdTry == 2)
            {
                Singleton.GetSession.SuccessOnThirdTry = 0;
                return ReturnSuccess();
            }

            Singleton.GetSession.SuccessOnThirdTry++;
            throw new TimeoutException();
        }


        private static IEnumerable<WeatherForecast> ReturnSuccess()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
