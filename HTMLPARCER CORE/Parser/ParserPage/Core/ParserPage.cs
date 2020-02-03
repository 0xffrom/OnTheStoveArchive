using AngleSharp.Html.Parser;
using System;
using RecipeLibrary.Objects;


namespace RecipeLibrary.Parse
{
    internal class ParserPage<T> where T : class
    {
        IParserPage<T> parser;
        IParserPageSettings parserSettings;

        HtmlLoader loader;

        internal IParserPage<T> Parser
        {
            get => parser;
            set => parser = value;
        }

        internal IParserPageSettings Settings
        {
            get => parserSettings;
            set {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        internal ParserPage(IParserPage<T> parser)
        {
            this.parser = parser;
        }

        internal ParserPage(IParserPage<T> parser, IParserPageSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }


        internal event Action<object, T> OnNewData;

        internal void StartParsePage(T obj)
        {
            if (obj.GetType().Name != "RecipeShort[]")
                throw new ParserException("Unknown object");
            Worker();

        }


        private static readonly Random random = new Random();
        private static int GetPageId(int maxPage) => random.Next(0, maxPage + 1);

        private async void Worker()
        {
            int maxPage = Settings.MaxPage;
            int pageId = GetPageId(maxPage);

            var source = await loader.GetSource(pageId);
            var domParser = new HtmlParser();
            var document = await domParser.ParseDocumentAsync(source);

            var result = parser.Parse(document);

            OnNewData?.Invoke(this, result);

        }
    }
}