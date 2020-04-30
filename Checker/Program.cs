using Newtonsoft.Json;
using ObjectsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checker
{
    class Program
    {
        static string queryRecipe = "http://51.77.58.18/recipe/get?url=";
        static string queryPage = "http://51.77.58.18/page/get?section=popular&page=";
        
        static async Task Main(string[] args)
        {
            for (int page = 1; page < 30000; page++)
            {
                Console.WriteLine("Страница: " + page);
                 List<RecipeShort> recipes = 
                    JsonConvert.DeserializeObject<List<RecipeShort>>
                    (GetSource(queryPage + page.ToString()).Result);
                foreach (var recipe in recipes)
                {
                    string result = await GetRecipeResult(queryRecipe + recipe.Url);
                    Console.WriteLine($"Сайт: {recipe.Url}. Результат: {result}");
                    await Logger(recipe.Url, result);
                }
            }
        }

        private static async Task Logger(string webSite, string result)
        {
            if (result.Contains("FALSE"))
            { 
            using StreamWriter streamWriter = new StreamWriter("log.log", true);
            await streamWriter.WriteLineAsync($"Сайт: {webSite}. Состояние: {result}");
            }
        }

        private static async Task<string> GetSource(string query)
        {
            string currentUrl = query;

            var client = new HttpClient();

            var response = await client.GetAsync(currentUrl);

            var source = string.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }


        private static async Task<string> GetRecipeResult(string query)
        {
            string currentUrl = query;

            var client = new HttpClient();

            var response = await client.GetAsync(currentUrl);

            var source = string.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                return ("TRUE");
            return ("FALSE");
        }
    }
}
