using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjectsLibrary;
using RecipeLibrary;
using System;
using WebServer.DataBase;

namespace WebServer.Controllers
{
    [ApiController]
    // Переадресация на {ip adress}/recipe/
    [Route("recipe")]
    public class IngredientController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public IngredientController(ILogger<PageController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Метод, возвращающий по GET запросу полный кулинарный рецепт. <see cref="RecipeFull"/>
        /// Маршрут: "{ip adress}/recipe/get?url={url}"
        /// </summary>
        /// <param name="url">URL адрес рецепта.</param>
        /// <returns>Объект типа RecipeFull</returns>
        [HttpGet("get")]
        public RecipeFull Get(string url)
        {
            DateTime startTime = DateTime.Now;
            _logger.LogInformation($"[{DateTime.Now}]: Запрос на парсинг старницы рецепта ===> {url}");
            try
            {

                var conn = Database.GetConnection();

                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    _logger.LogDebug($"[{DateTime.Now}]: Успешное подключение к базе данных.");
                else
                    _logger.LogError($"[{DateTime.Now}]: Неудалось подключиться к базе данных.");

                RecipeFull recipe;

                if (Database.IsNeedUpdate(url, conn))
                {
                    Database.AddRecipe(url, recipe = GetData.GetRecipe(url).Result, conn);
                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт был получен путём парсинга страницы.");
                }
                else
                {
                    recipe = Database.GetRecipe(url, conn, out double size);
                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                }

                conn.Close();

                _logger.LogDebug($"[{DateTime.Now}]: Отключение от базы данных.");

                _logger.LogInformation($"[{DateTime.Now}]: Запрос успешно выполнен.");
                _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                return recipe;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[{DateTime.Now}]: Запрос выполнен неудачно.");
                return new RecipeFull();
            }
        }
    }
}