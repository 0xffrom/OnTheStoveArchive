using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            RecipeFull obj = GetPage("https://www.povarenok.ru/recipes/show/52345/");
            Console.WriteLine(JsonConvert.SerializeObject(obj));
            
            Console.WriteLine(obj.IngredientsBoxes[0].Ingredients[0].Name);
        }

        private static async Task<string> GetSource(string query)
        {
            string currentUrl = "http://194.87.103.195/" + query;

            var client = new HttpClient();

            var response = await client.GetAsync(currentUrl);

            var source = string.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            
            return source;
            
        }


        private static RecipeFull GetPage(string url) =>
            JsonConvert.DeserializeObject<RecipeFull>
                (GetSource("getRecipe?url=" + url).Result);
    }
}