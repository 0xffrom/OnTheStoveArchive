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
            _client = new HttpClient();
            _url = settings.Url;
        }

        public async Task<string> GetSource(int idPage, string recipeName)
        {
            string currentUrl = _url;

            switch (_settings.Section)
            {
                case ("new"):
                    currentUrl += _settings.SuffixNew;
                    break;
                case ("popular"):
                    currentUrl += _settings.SuffixPopular;
                    break;
                case ("recipe"):
                    currentUrl += _settings.SuffixRecipe;
                    break;
                default:
                    // Если есть поиск по тегу:
                    if (_settings.Sections.ContainsKey(_settings.Section))
                    {
                        currentUrl += _settings.Sections[_settings.Section];
                        break;
                    }
                    
                    currentUrl += _settings.SuffixNew;
                    break;
            }

            currentUrl = currentUrl
                .Replace("{PageId}", idPage.ToString())
                .Replace("{RecipeName}", recipeName);

            var response = await _client.GetAsync(currentUrl);

            string source;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            else
                throw new ParserException("Произошла ошибка при загрузке сайта");

            return source;
        }
    }
}