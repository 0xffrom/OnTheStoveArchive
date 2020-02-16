using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RecipeLibrary.Parse;

namespace RecipeLibrary.Parser.ParserRecipe.Core
{
    public class HtmlLoader
    {
        private readonly HttpClient client;
        private readonly string url;

        public HtmlLoader(IParserRecipeSettings settings)
        {
            client = new HttpClient();
            url = settings.Url;
        }

        public async Task<string> GetSource()
        {
            var currentUrl = url;

            var response = await client.GetAsync(currentUrl);

            string source;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();
            else
                throw new ParserException("Error loading page");

            return source;
        }
    }
}