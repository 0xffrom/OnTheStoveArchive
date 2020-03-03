using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using RecipeLibrary.Objects;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        public static async Task<string> GetSource()
        {
            string currentUrl = "http://45.132.17.35/getPage?section=random";
            var client = new HttpClient();

            string source = string.Empty;

            var response = await client.GetAsync(currentUrl);

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }

        static void Main(string[] args)
        {
            string source = GetSource().Result;

            List<RecipeShort> recipes = JsonConvert.DeserializeObject<List<RecipeShort>>(source);
            foreach (var item in recipes)
                Console.WriteLine($"{item.Title} - {item.Url}. Picture: {item.Picture.Url}");
            
        }
    }
}
