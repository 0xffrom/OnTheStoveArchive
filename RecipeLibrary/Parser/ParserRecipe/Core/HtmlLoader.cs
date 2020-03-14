using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecipeLibrary.Parser.ParserRecipe.Core
{
    public class HtmlLoader
    {
        private readonly HttpClient _client;
        public HtmlLoader(IParserRecipeSettings settings)
        {
            _client = new HttpClient();
        }

        internal async Task<string> GetSource(string url)
        {
            var currentUrl = url;
            var response = await _client.GetAsync(currentUrl);

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