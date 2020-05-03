using AngleSharp.Html.Parser;
using System;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserPage.Core
{
    internal class ParserPage<T> where T : class
    {
        private IParserPageSettings _parserSettings;

        private HtmlLoader _loader;
        private IParserPage<T> Parser { get; }

        private IParserPageSettings Settings
        {
            get => _parserSettings;
            set {
                _parserSettings = value;
                _loader = new HtmlLoader(value);
            }
        }

        private ParserPage(IParserPage<T> parser)
        {
            Parser = parser;
        }

        internal ParserPage(IParserPage<T> parser, IParserPageSettings parserSettings) : this(parser)
        {
            Settings = parserSettings;
        }

        private static readonly Random Random = new Random();
        private static int GetPageId(int maxPage) => Random.Next(1, maxPage + 1);

        internal async Task<T> Worker()
        {
            var pageId = Settings.PageId;

            if (Settings.Section == "random")
                pageId = GetPageId(Settings.MaxPageId);

            string recipeName = Settings.RecipeName;

            string source = await _loader.GetSource(pageId, recipeName);

            // Если нет существуеющего раздела у сайта - выкидывается пустой.
            if (source == null)
                return null;

            HtmlParser domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            var result = Parser.Parse(document, _parserSettings);

            return result;
        }
    }
}