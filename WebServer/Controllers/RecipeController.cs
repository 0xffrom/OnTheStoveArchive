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
        public async Task<ActionResult> Get(string url)
        {
            GC.Collect();
            DateTime startTime = DateTime.Now;
            MySql.Data.MySqlClient.MySqlConnection conn = RecipeDataContext.GetConnection();
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
                case System.Data.ConnectionState.Open when RecipeDataContext.IsNeedUpdate(url, conn):
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
                        if (RecipeDataContext.IsExists(url, conn))
                        {
                            _logger.LogWarning($"[{DateTime.Now}]: Был выброшен старый рецепт.");
                            recipe = RecipeDataContext.GetRecipe(url, conn, out double size);
                            _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                        }

                        // Иначе выкидываем пустой рецепт:
                        else
                        {
                            _logger.LogCritical($"[{DateTime.Now}]: В базе данных отсутствует данный рецепт. Был выброшен пустой рецепт.");
                            _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                            _logger.LogCritical($"[{DateTime.Now}]");
                            return NoContent();
                        }

                        _logger.LogDebug($"[{DateTime.Now}]: Отключение от базы данных.");
                        _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");

                        conn.Clone();
                        return Ok(recipe);

                    }

                    // Если ошибки нет, добавляем рецепт в БД:
                    RecipeDataContext.AddRecipe(url, recipe, conn);

                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт был получен путём парсинга страницы, и был добавлен в БД.");
                    _logger.LogDebug($"[{DateTime.Now}] Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                    return Ok(recipe);
                
                // Подключение к БД есть и рецепт не нуждается в обновлении:
                case System.Data.ConnectionState.Open:
                    _logger.LogDebug($"[{DateTime.Now}]: Рецепт не нуждается в обновлении.");

                    try
                    {
                        recipe = RecipeDataContext.GetRecipe(url, conn, out double size);

                        _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                    }
                    // Если возникает ошибка с БД:
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        _logger.LogCritical($"[{DateTime.Now}]: Ошибка с БД. Был выброшен пустой рецепт.");
                        _logger.LogDebug($"[{DateTime.Now}]: Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        return NoContent();
                    }
                    finally
                    {
                        _logger.LogDebug($"[{DateTime.Now}]: Отключение от базы данных.");
                        conn.Close();
                    }

                    return Ok(recipe);
                // Подключения к БД нет:
                default:
                    try
                    {       
                        // Пытаемся запарсить рецепт:
                        recipe = await GetData.GetRecipe(url);
                        _logger.LogDebug($"[{DateTime.Now}]: Рецепт получен.");
                        _logger.LogDebug($"[{DateTime.Now}]: Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        return Ok(recipe);
                    }
                    
                    // Если случается ошибка при парсинге сайта:
                    catch (Exception exp)
                    {
                        if (exp is ParserException)
                            _logger.LogError(exp, $"[{DateTime.Now}]: Рецепт не парсится.");
                        else
                            _logger.LogCritical(exp, $"[{DateTime.Now}]: Рецепт не парсится. Ошибка неизвестна.");
                        
                        _logger.LogDebug($"[{DateTime.Now}]: Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        _logger.LogCritical($"[{DateTime.Now}]: Ошибка с БД и в парсинге рецепта. Был выброшен пустой рецепт.");
                        return NoContent();
                    }
            }         
        }
    }
}