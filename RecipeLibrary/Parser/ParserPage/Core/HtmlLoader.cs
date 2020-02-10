using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RecipeLibrary.Parse;
using RecipeLibrary.Parser.ParserPage.Core;
using static System.Text.Encoding;

namespace RecipeLibrary.ParsePage
{
    public class HtmlLoader
    {
        private readonly HttpClient client;
        private readonly string url;

        private IParserPageSettings _settings;
        public HtmlLoader(IParserPageSettings settings)
        {
            _settings = settings;
            client = new HttpClient();
            url = settings.Url;
        }

        public async Task<string> GetSource(int idPage, string recipeName)
        {
            string currentUrl;

            currentUrl = url;
            Console.WriteLine(_settings);
            switch (_settings.Section)
            {
                case("new"):
                    currentUrl += _settings.SuffixNew;
                    break;
                case("popular"):
                    currentUrl += _settings.SuffixPopular;
                    break;
                case("recipe"):
                    Console.WriteLine("Я тут");
                    currentUrl += _settings.SuffixRecipe;
                    break;
            }
            currentUrl = currentUrl
                .Replace("{PageId}", idPage.ToString())
                .Replace("{RecipeName}", recipeName);

            Console.WriteLine($"Parsing URL: {currentUrl}");
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