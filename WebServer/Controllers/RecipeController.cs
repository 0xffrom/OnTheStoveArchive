using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjectsLibrary;
using ObjectsLibrary.Parser;
using RecipeLibrary;
using System;
using System.Threading.Tasks;
using WebServer.DataBase;
namespace WebServer.Controllers
{
    [ApiController]
    // Переадресация на {ip adress}/recipe/
    [Route("recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(ILogger<RecipeController> logger)
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
        public async Task<RecipeFull> Get(string url)
        {

            DateTime startTime = DateTime.Now;
            MySql.Data.MySqlClient.MySqlConnection conn = Database.GetConnection();
            _logger.LogInformation($"[{DateTime.Now}]: Запрос на парсинг старницы рецепта. Url: {url}");

            try
            {       
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                _logger.LogError($"[{DateTime.Now}]: Неудалось подключиться к базе данных.");
            }

            if (conn.State == System.Data.ConnectionState.Open)
                _logger.LogDebug($"[{DateTime.Now}]: Успешное подключение к базе данных.");

            RecipeFull recipe;

            switch (conn.State)
            {
                // Подключение к БД есть и рецепт нуждается в обновлении:
                case System.Data.ConnectionState.Open when Database.IsNeedUpdate(url, conn):
                    try
                    {
                        // Пытаемся запарсить рецепт:
                        recipe = await GetData.GetRecipe(url);
                    }
                    // Если случается ошибка при парсинге сайта:
                    catch (Exception exp)
                    {
                        if (exp is ParserException)
                            _logger.LogError(exp, $"[{DateTime.Now}]: Рецепт не парсится.");
                        else
                            _logger.LogCritical(exp, $"[{DateTime.Now}]: Рецепт не парсится. Ошибка неизвестна.");


                        // Если рецепт есть в БД, выкидываем его:
                        if (Database.IsExists(url, conn))
                        {
                            _logger.LogWarning($"[{DateTime.Now}]: Был выброшен старый рецепт.");
                            recipe = Database.GetRecipe(url, conn, out double size);
                            _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                        }

                        // Иначе выкидываем пустой рецепт:
                        else
                        {
                            _logger.LogCritical($"[{DateTime.Now}]: В базе данных отсутствует данный рецепт. Был выброшен пустой рецепт.");
                            recipe = new RecipeFull();
                        }

                        _logger.LogDebug($"[{DateTime.Now}]: Отключение от базы данных.");
                        conn.Clone();

                        return ReturnRecipe(startTime, recipe, true);

                    }

                    // Если ошибки нет, добавляем рецепт в БД:
                    Database.AddRecipe(url, recipe, conn);

                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт был получен путём парсинга страницы, и был добавлен в БД.");
                    return ReturnRecipe(startTime, recipe, false);
                
                // Подключение к БД есть и рецепт не нуждается в обновлении:
                case System.Data.ConnectionState.Open:
                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт не нуждается в обновлении.");

                    try
                    {
                        recipe = Database.GetRecipe(url, conn, out double size);

                        _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                    }
                    // Если возникает ошибка с БД:
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        _logger.LogCritical($"[{DateTime.Now}]: Ошибка с БД. Был выброшен пустой рецепт.");

                        recipe = new RecipeFull();

                        return ReturnRecipe(startTime, recipe, true);
                    }
                    finally
                    {
                        _logger.LogDebug($"[{DateTime.Now}]: Отключение от базы данных.");
                        conn.Close();
                    }

                    return ReturnRecipe(startTime, recipe, false);
                // Подключения к БД нет:
                default:
                    try
                    {
                        // Пытаемся запарсить рецепт:
                        recipe = await GetData.GetRecipe(url);
                        _logger.LogDebug($"[{DateTime.Now}]: Рецепт получен.");
                    }
                    // Если случается ошибка при парсинге сайта:
                    catch (Exception exp)
                    {
                        if (exp is ParserException)
                            _logger.LogError(exp, $"[{DateTime.Now}]: Рецепт не парсится.");
                        else
                            _logger.LogCritical(exp, $"[{DateTime.Now}]: Рецепт не парсится. Ошибка неизвестна.");


                        _logger.LogCritical($"[{DateTime.Now}]: Ошибка с БД и в парсинге рецепта. Был выброшен пустой рецепт.");

                        recipe = new RecipeFull();

                        return ReturnRecipe(startTime, recipe, true);
                    }
                    return ReturnRecipe(startTime, recipe, false);
            }         
        }

        /// <summary>
        /// Логирование выдачи рецепта.
        /// </summary>
        /// <param name="startTime">Стартовое время.</param>
        /// <param name="recipe">Рецепт, который нужно вернуть.</param>
        /// <param name="isError">Была ли ошибка?</param>
        /// <returns>Тот же самый <param name="recipe"/></returns>
        private RecipeFull ReturnRecipe(DateTime startTime, RecipeFull recipe, bool isError = false)
        {
            if (isError)
                _logger.LogWarning($"[{DateTime.Now}]: Запрос выполнен c ошибками.");
            else
                _logger.LogInformation($"[{DateTime.Now}]: Запрос выполнен успешно.");

            _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
            return recipe;
        }
    }
}