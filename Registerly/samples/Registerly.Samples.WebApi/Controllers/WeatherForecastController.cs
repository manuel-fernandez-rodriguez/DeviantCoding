using Microsoft.AspNetCore.Mvc;
using Registerly.Samples.WebApi.Services;

namespace Registerly.Samples.WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IWeatherService weatherService) : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get() => weatherService.Get();
}
