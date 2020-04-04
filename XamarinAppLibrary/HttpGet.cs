using Newtonsoft.Json;
using ObjectsLibrary;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamarinAppLibrary
{

    public static class HttpGet
    {
        private const string ipAdress = "http://194.87.103.195/";
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

        public static RecipeShort[] GetRecipes(string query) =>
            JsonConvert.DeserializeObject<RecipeShort[]>(GetSource(query).Result);

        public static RecipeFull GetPage(string url) =>
            JsonConvert.DeserializeObject<RecipeFull>
                (GetSource("getRecipe?url=" + url).Result);
    }
}