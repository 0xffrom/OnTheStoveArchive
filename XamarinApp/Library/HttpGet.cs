
using Newtonsoft.Json;
using RecipesAndroid.Objects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipesAndroid
{

    public static class HttpGet
    {
        private static async Task<string> GetSource()
        { 
            string currentUrl = "http://194.87.103.195/getPage?section=random";

            var client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());

            var response = await client.GetAsync(currentUrl);

            string source = string.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }

        public static List<RecipeShort> GetRecipes()
        {
            string source = GetSource().Result;

            List<RecipeShort> recipes = JsonConvert.DeserializeObject<List<RecipeShort>>(source);

            return recipes;

        }
    }
}
