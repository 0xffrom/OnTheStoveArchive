using Newtonsoft.Json;
using ObjectsLibrary;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamarinAppLibrary
{

    public static class HttpGet
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

            
            return source;
        }

        public static List<RecipeShort> GetPages(string query) =>
            JsonConvert.DeserializeObject<List<RecipeShort>>(GetSource("page/get?" + query).Result);

        public static RecipeFull GetRecipe(string url) =>
            JsonConvert.DeserializeObject<RecipeFull>
                (GetSource("recipe/get?url=" + url).Result);
    }
}