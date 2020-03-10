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
    public class PageController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public PageController(ILogger<PageController> logger)
        {
            _logger = logger;
        }


        [HttpGet("getPage")]
        public async Task<IEnumerable<RecipeShort>> Get(string section, int page = 1, string recipeName = null)
        {
            if (recipeName == null)
                recipeName = string.Empty;

            GetData getData = new GetData();
            try
            {
                RecipeShort[] recipeShorts =  getData.GetPage(section, page, recipeName.ToLower());
                Console.WriteLine("Вернул");
                return Enumerable.Range(1, recipeShorts.Length).Select(index => recipeShorts[index - 1])
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