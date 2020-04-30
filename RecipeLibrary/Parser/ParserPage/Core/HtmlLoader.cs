using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserPage.Core
{
    public class HtmlLoader
    {
        private readonly HttpClient _client;
        private readonly string _url;

        private readonly IParserPageSettings _settings;

        public HtmlLoader(IParserPageSettings settings)
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

            var response = await _client.GetAsync(currentUrl);

            string source;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            else
                throw new ParserException("Произошла ошибка при загрузке сайта");

            return source;
        }
    }
}