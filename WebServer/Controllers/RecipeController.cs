using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjectsLibrary;
using RecipeLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Data;

namespace WebServer.Controllers
{
    [ApiController]
    // Переадресация на {ip adress}/recipe/
    [Route("recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private const int diffHours = 24;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecipe(string url)
        {
            _logger.LogInformation($"Запрос на парсинг старницы рецепта. Url: {url}");

            DateTime startTime = DateTime.Now;
            RecipeFull recipe;
            Recipe recipeDb;

            using(RecipeContext db = new RecipeContext())
            {
                // Если в БД есть рецепт: 
                if((recipeDb = db.Recipes.FirstOrDefault(x => x.Url == url)) != null)
                {
                    _logger.LogDebug($"Рецепт существует в БД. Id = {recipeDb.Id}, Date = {recipeDb.Date}");
                    // Если рецепт нужно обновить: 
                    if ((startTime - recipeDb.Date).TotalHours > diffHours)
                    {
                        _logger.LogDebug($"Требуется обновление рецепта.");
                        try
                        {
                            recipe = await GetData.GetRecipe(url);

                            recipeDb.RecipeFull = recipe;
                            recipeDb.Date = startTime;
                            db.SaveChanges();

                            _logger.LogInformation($"Статус: Ok.");
                            LogTime(startTime);
                            return Ok(recipe);
                        }
                        catch(Exception e)
                        {
                            _logger.LogWarning(e, $"Произошла ошибка при парсинге рецепта! Выдан рецепт из БД.");
                            _logger.LogInformation($"Статус: Ok.");

                            LogTime(startTime);
                            return Ok(recipeDb.RecipeFull);
                        }
                    }

                    // Если рецепт есть в БД и обновляеть его ненужно:
                    else
                    {
                        _logger.LogDebug("Обновление не требуется.");
                        _logger.LogInformation($"Статус: Ok.");

                        LogTime(startTime);
                        return Ok(recipeDb.RecipeFull);
                    }
                }
                // Если в БД нет рецепта:
                else
                {
                    _logger.LogDebug($"Рецепта не существует в БД.");
                    try
                    {
                        recipe = await GetData.GetRecipe(url);
                        db.Recipes.Add(new Recipe { Url = url, Date = startTime, RecipeFull = recipe }) ;
                        db.SaveChanges();

                        _logger.LogInformation($"Статус: Ok.");
                        LogTime(startTime);
                        return Ok(recipe);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, $"Произошла ошибка при парсинге рецепта!");
                        _logger.LogInformation($"Статус: 400");

                        LogTime(startTime);
                        return BadRequest();
                    }
                }
            }
        }

        private void LogTime(DateTime startTime)
        {
            _logger.LogDebug($"Время исполнения: {(DateTime.Now - startTime).TotalMilliseconds} миллисекунд.");
        }
    }
}