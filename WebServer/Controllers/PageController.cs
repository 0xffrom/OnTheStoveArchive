using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeLibrary.Objects;
using RecipeLibrary;
using RecipeLibrary.Objects.Boxes.Elements;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PageController> _logger;

        public PageController(ILogger<PageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<RecipeShort> Get()
        {
            var rng = new Random();
           // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
           //     {
           //        Date = DateTime.Now.AddDays(index),
           //       TemperatureC = rng.Next(-20, 55),
           //       Summary = Summaries[rng.Next(Summaries.Length)]
           //   })
           //   .ToArray();
           //RecipeShort[] recipeShorts = GetPage("1", "1", 1);
           
           RecipeShort[] recipeShorts = new RecipeShort[2]
           {
               new RecipeShort("TEST1", new Picture("url picture"), "urlchik" ),
               new RecipeShort("TEST2", new Picture("urlochka"), "ssilka" )
           };
           
           // TODO: Тебе нужно изучить таски, чтобы ждать ассинхроннго выполнения метода.
           return Enumerable.Range(1, recipeShorts.Length).Select(index => recipeShorts[index-1]).ToArray();
        }
    }
}