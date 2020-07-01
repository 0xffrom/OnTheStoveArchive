using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserRecipe.Core
{
    public class HtmlLoaderRecipe
    {
        private readonly HttpClient _client;

        public HtmlLoaderRecipe(IParserRecipeSettings settings)
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