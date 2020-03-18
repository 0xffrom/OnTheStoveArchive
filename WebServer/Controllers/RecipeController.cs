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
        public RecipeFull Get(string url)
        {
            Console.WriteLine(url);
            try
            {
                // TODO: Починить рецепт 5659 поварёнок.
                return GetData.GetRecipe(url).Result;
            }
            catch (Exception e )
            {
                Console.WriteLine(e);
                return new RecipeFull(null, null, null,
                    null, null, null);
            }
        }
    }
}