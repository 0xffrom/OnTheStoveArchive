using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeLibrary.Objects;
using RecipeLibrary;
using RecipeLibrary.Objects.Boxes.Elements;

namespace WebServer.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public RecipeController(ILogger<PageController> logger)
        {
            _logger = logger;
        }

        // RESPONSE: ?section=<section>&recipeName=<recipe>&page=<page>
        //           ?recipeName=<recipe>&section=<section>&page=<page>
        [HttpGet("getRecipe")]
        public IEnumerable<RecipeShort> Get(string url)
        {
            GetData getData = new GetData();
            try
            {
                getData.GetRecipe(url);
                while (!getData.IsCompleted)
                {
                    // TODO: Переделать этот ужасный костыль.
                }

                return Enumerable.Range(1, getData.RecipeShorts.Count).Select(index => getData.RecipeShorts[index - 1])
                    .ToArray();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);

                return Enumerable.Range(1, 1)
                    .Select(index => new RecipeShort("error", new Picture("error"), "error"))
                    .ToArray();
            }
        }
    }
}