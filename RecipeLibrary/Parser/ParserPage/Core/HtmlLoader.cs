using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.Core
{
    public class HtmlLoader
    {
        private readonly HttpClient _client;
        private readonly string _url;

        private readonly IParserPageSettings _settings;

        public HtmlLoader(IParserPageSettings settings)
        {
            _settings = settings;
            _client = new HttpClient();
            _url = settings.Url;
        }

        public async Task<string> GetSource(int idPage, string recipeName)
        {
            var currentUrl = _url;

            currentUrl += _settings.Section switch
            {
                ("new") => _settings.SuffixNew,
                ("popular") => _settings.SuffixPopular,
                ("recipe") => _settings.SuffixRecipe,
                _ => _settings.SuffixNew
            };
            
            currentUrl = currentUrl
                .Replace("{PageId}", idPage.ToString())
                .Replace("{RecipeName}", recipeName);
            
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