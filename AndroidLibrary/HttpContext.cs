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

        public static List<RecipeShort> GetPages(string query) =>
            JsonConvert.DeserializeObject<List<RecipeShort>>(GetSource("page/get?" + query).Result);

        public static RecipeFull GetRecipe(string url) =>
            JsonConvert.DeserializeObject<RecipeFull>(GetSource("recipe/get?url=" + url).Result);
    }
}