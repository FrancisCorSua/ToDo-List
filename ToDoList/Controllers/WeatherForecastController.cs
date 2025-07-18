using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers.WeatherForecast;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController: ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    [HttpGet]
    public IEnumerable<Models.WeatherForecast.WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index =>
                new Models.WeatherForecast.WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            .ToArray();
    }
}