using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RecipeLibrary.Parse;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.ParsePage
{
    public class HtmlLoader
    {
        private readonly HttpClient client;
        private readonly string url;

        public HtmlLoader(IParserPageSettings settings)
        {
            client = new HttpClient();
            url = settings.Url;
        }

        public async Task<string> GetSource(int idPage, string recipeName)
        {
            var currentUrl = url
                .Replace("{IdPage}", idPage.ToString())
                .Replace("{RecipeName}", recipeName);
            
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
