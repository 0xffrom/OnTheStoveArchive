using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebServer.DataBase;
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
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"Запрос на парсинг старницы рецепта ===> {url}");
            try
            {
                var conn = Database.GetConnection();
                
                conn.Open();
                
                RecipeFull recipe;
                if (Database.IsNeedUpdate(url, conn))
                    Database.AddRecipe(url, recipe = GetData.GetRecipe(url).Result, conn);
                else
                    recipe = Database.GetRecipe(url, conn);
                
                conn.Close();
                
                recipe.Url = url;
                Console.WriteLine($"Запрос выполнен успешно за {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                return recipe;
            }
            catch (Exception e )
            {
                Console.WriteLine($"Запрос выполнен неудачно. Ошибка: {e}");
                return new RecipeFull();
            }
        }
    }
}