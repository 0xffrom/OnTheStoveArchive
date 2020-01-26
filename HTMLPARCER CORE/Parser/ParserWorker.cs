using AngleSharp.Html.Parser;
using System;


namespace RecipeLibrary.Parse
{
    internal class ParserWorker<T> where T : class
    {
        IParser<T> parser;
        IParserSettings parserSettings;

        HtmlLoader loader;

        internal IParser<T> Parser
        {
            get => parser; 
            set => parser = value; 
        }

        internal IParserSettings Settings
        {
            get => parserSettings;
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        internal ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        internal ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        internal event Action<object, T> OnNewData;

        internal void Start() => Worker();


        internal static readonly Random random = new Random();
        internal static int GetPageId(int maxPage) => random.Next(0, maxPage + 1);


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