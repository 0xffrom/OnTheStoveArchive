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
            _logger.LogInformation($"Запрос на парсинг старницы рецепта. Url: {url}");

            try
            { 
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                _logger.LogError("Неудалось подключиться к базе данных.");
            }

            if (conn.State == System.Data.ConnectionState.Open)
                _logger.LogDebug("Успешное подключение к базе данных.");

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
                        _logger.LogError(exp, "Рецепт невозможно получить с сайта.");        

                        // Если рецепт есть в БД, выкидываем его:
                        if (RecipeDataContext.IsExists(url, conn))
                        {
                            _logger.LogWarning($"[{DateTime.Now}]: Был выдан старый рецепт.");
                            recipe = RecipeDataContext.GetRecipe(url, conn, out double size);
                            _logger.LogDebug($"[{DateTime.Now}]: Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                        }

                        // Иначе выкидываем пустой рецепт:
                        else
                        {
                            _logger.LogCritical($"В базе данных отсутствует данный рецепт. Был выброшен пустой рецепт.");
                            _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                            _logger.LogInformation($"Статус: NotFound.");
                            return NotFound();
                        }

                        _logger.LogDebug($"Отключение от базы данных.");
                        _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        _logger.LogInformation($"Статус: Ok.");

                        conn.Clone();
                        return Ok(recipe);
                    }

                    // Если ошибки нет, добавляем рецепт в БД:
                    RecipeDataContext.AddRecipe(url, recipe, conn);

                    _logger.LogDebug($"Рецепт был получен путём парсинга страницы, и был добавлен в БД.");
                    _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                    _logger.LogInformation($"Статус: Ok.");
                    return Ok(recipe);
                
                // Подключение к БД есть и рецепт не нуждается в обновлении:
                case System.Data.ConnectionState.Open:

                    _logger.LogDebug($"Рецепт не нуждается в обновлении.");

                    try
                    {
                        recipe = RecipeDataContext.GetRecipe(url, conn, out double size);
                        _logger.LogDebug($"Рецепт взят из базы данных. Размер рецепта: {size:F2} KB");
                    }
                    // Если возникает ошибка с БД:
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        _logger.LogCritical($"Ошибка с БД. Был выброшен пустой рецепт.");
                        _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        _logger.LogInformation($"Статус: NotFound.");
                        return NotFound();
                    }
                    finally
                    {
                        _logger.LogDebug($"Отключение от базы данных.");
                        conn.Close();
                    }

                    _logger.LogInformation($"Статус: Ok.");
                    return Ok(recipe);
                // Подключения к БД нет:
                default:
                    try
                    {       
                        // Пытаемся запарсить рецепт:
                        recipe = await GetData.GetRecipe(url);
                        _logger.LogDebug($"Рецепт получен.");
                        _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        _logger.LogInformation($"Статус: Ok.");
                        return Ok(recipe);
                    }
                    
                    // Если случается ошибка при парсинге сайта:
                    catch (Exception exp)
                    {
                        _logger.LogError(exp, "Рецепт невозможно получить с сайта.");
                        _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
                        _logger.LogCritical($"Ошибка с БД и в парсинге рецепта. Был выброшен пустой рецепт.");
                        _logger.LogInformation($"Статус: NotFound.");
                        return NotFound();
                    }
            }         
        }
    }
}