using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjectsLibrary;
using RecipeLibrary;
using System;

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
        public async System.Threading.Tasks.Task<RecipeShort[]> Get(string section, int page = 1, string recipeName = null)
        {
            recipeName ??= string.Empty;

            DateTime startTime = DateTime.Now;

            _logger.LogInformation($"[{DateTime.Now}]: Получен запрос на парсинг страницы с рецептами ==> " +
                $"section: {section}, page: {page}, recipeName: {recipeName}");

            try
            {
                RecipeShort[] recipes = await GetData.GetPage(section.ToLower(), page, recipeName.ToLower());

                _logger.LogInformation($"[{DateTime.Now}]: Запрос успешно выполнен.");
                _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                return recipes;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[{DateTime.Now}]: Запрос выполнен неудачно.");
                _logger.LogWarning($"[{DateTime.Now}]: Возврат нулевого рецепта.");
                return new RecipeShort[] { new RecipeShort() };
            }
        }
    }
}