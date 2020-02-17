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
        
        [HttpGet("getRecipe")]
        public IEnumerable<RecipeFull> Get(string url)
        {
            Console.WriteLine(url);
            try
            {
                GetData getData = new GetData();

                getData.GetRecipe(url);
                while (!getData.IsCompleted)
                {
                    // TODO: Переделать этот ужасный костыль.
                }

                return Enumerable.Range(1, 1)
                    .Select(index => getData.RecipeFull)
                    .ToArray();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);

                return Enumerable.Range(1, 1)
                    .Select(index => new RecipeFull(null, null, null, null, null, null))
                    .ToArray();
            }
        }
    }
}