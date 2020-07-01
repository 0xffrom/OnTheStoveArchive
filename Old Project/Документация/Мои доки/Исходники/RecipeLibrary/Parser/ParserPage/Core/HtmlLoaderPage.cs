using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserPage.Core
{
    public class HtmlLoaderPage
    {
        private readonly HttpClient _client;
        private readonly string _url;
        private readonly IParserPageSettings _settings;
        public HtmlLoaderPage(IParserPageSettings settings)
        {
            _settings = settings;
            _url = settings.Url;
            _client = new HttpClient();
        }

        public async Task<string> GetSource(int idPage, string recipeName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (!_settings.Sections.ContainsKey(_settings.Section))
            {
                throw new ParserException($"Раздела '{_settings.Section}' не существует.");
            }

            string currentUrl = _url + _settings.Sections[_settings.Section];

            currentUrl = currentUrl
                .Replace("{PageId}", idPage.ToString())
                .Replace("{RecipeName}", recipeName);

            HttpResponseMessage response;

            // Для сайта eda.ru выполняются POST запросы, для остальных GET.
            if (currentUrl.Contains("https://eda.ru"))
            {
                var data = "name=onthestove";
                StringContent queryString = new StringContent(data);
                response = await _client.PostAsync(new Uri(currentUrl), queryString);

                string responseBody = string.Empty;
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                    responseBody = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseBody);

                return jObject.ContainsKey("Recipes") ?
                    jObject["Recipes"]?.Value<string>() : jObject["Html"].Value<string>();
            }
            
            response = await _client.GetAsync(currentUrl);

            string source;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            else
                throw new ParserException("Произошла ошибка при загрузке сайта");

            return source;
        }
    }
}