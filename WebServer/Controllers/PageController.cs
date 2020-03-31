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
        public List<RecipeShort> Get(string section, int page = 1, string recipeName = null)
        {
            recipeName ??= string.Empty;
            
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"Запрос на парсинг страницы с рецептами ===> {section}");
            
            try
            {
                Console.WriteLine($"Запрос выполнен успешно за {(DateTime.Now - startTime).Milliseconds} миллисекунд.");
                return GetData.GetPage(section, page, recipeName.ToLower()).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Запрос выполнен неудачно. Ошибка: {e}");
                return new List<RecipeShort> {new RecipeShort("error", new Picture("error"), "error")};
            }
        }
    }
}