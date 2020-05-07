using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjectsLibrary;
using RecipeLibrary;
using System;
using System.Threading.Tasks;

namespace WebServer.Controllers
{
    [ApiController]
    // Переадресация на {ip adress}/page/
    [Route("page")]
    public class PageController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public PageController(ILogger<PageController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Метод, возвращающий по GET запросу коллекцию кратких описаний кулинарных рецептов. <see cref="RecipeShort"/>
        /// Маршрут: "{ip adress}/page/get?section={section}&page={page}&recipeName={recipeName}"
        /// Параметры "page" и "recipeName" является необязательными.
        /// </summary>
        /// <param name="section">Раздел рецепта.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="recipeName">Название рецепта.</param>
        /// <returns>Объект типа RecipeShort[]</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetPages(string section, int page = 1, string recipeName = null)
        {
            string log = $"GET запрос на получение страниц с рецептами. Параметры: " +
                         $"section={section}, page={page}";

            if (recipeName != null)
            {
                log += $", recipeName={recipeName}";
            }
            else
            {
                recipeName = string.Empty;
            }

            _logger.LogInformation(log);
            DateTime startTime = DateTime.Now;

            try
            {
                RecipeShort[] recipes = await GetData.GetPage(section.ToLower(), page, recipeName.ToLower());

                _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                _logger.LogInformation($"Статус: Ok.");

                LogTime(startTime);
                return Ok(recipes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Запрос выполнен неудачно.");
                _logger.LogInformation($"Статус: 400.");

                LogTime(startTime);
                return BadRequest();
            }
        }
        private void LogTime(DateTime startTime)
        {
            _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
        }
    }
}