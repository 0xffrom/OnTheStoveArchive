using Newtonsoft.Json;
using ObjectsLibrary;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndroidLibrary
{
    public static class HttpContext
    {
        //http://51.77.58.18/
        private const string ipAdress = "http://51.77.58.18/";

        private static async Task<string> GetSource(string query)
        {
            string currentUrl = ipAdress + query;

            var client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());

            var response = await client.GetAsync(currentUrl);

            var source = string.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                throw new HttpRequestException("Не удалось загрузить рецепт.");
            if (source == string.Empty)
                throw new HttpRequestException("Нет подключения к интернету, попробуйте позже :(");

            return source;
        }

        public static List<RecipeShort> GetPages(string query)
        {
            string json = GetSource("page/get?" + query).Result;
            var obj = JsonConvert.DeserializeObject<List<RecipeShort>>(json);
            return obj;
        }

        public static RecipeFull GetRecipe(string url)
        {
            string json = GetSource("recipe/get?url=" + url).Result;
            var obj = JsonConvert.DeserializeObject<RecipeFull>(json);
            return obj;
        }
    }
}