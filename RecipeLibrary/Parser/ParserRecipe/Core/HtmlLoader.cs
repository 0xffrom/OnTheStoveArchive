using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RecipeLibrary.Parse;

namespace RecipeLibrary.Parser.ParserRecipe.Core
{
    public class HtmlLoader
    {
        private readonly HttpClient client;
        public HtmlLoader(IParserRecipeSettings settings)
        {
            client = new HttpClient();
        }

        internal async Task<string> GetSource(string url)
        {
            var currentUrl = url;
            var response = await client.GetAsync(currentUrl);

            string source;
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();
            else
                throw new ParserException("Error loading page");

            return source;
        }
    }
}