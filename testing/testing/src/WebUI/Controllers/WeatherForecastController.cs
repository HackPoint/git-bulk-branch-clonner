using testing.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Mvc;

namespace testing.WebUI.Controllers;

public class WeatherForecastController : ApiControllerBase {
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get() {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}