using System;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using RecipeLibrary.Parse;
using RecipeLibrary.ParsePage;

namespace RecipeLibrary.Parser.ParserPage.Core
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
            Parser = parser;
        }

        internal ParserPage(IParserPage<T> parser, IParserPageSettings parserSettings) : this(parser)
        {
            this.Settings = parserSettings;
        }
        
        internal event EventHandler<T> OnNewData;

        internal void StartParsePage() => Worker();
        
        private static readonly Random random = new Random();
        private static int GetPageId(int maxPage) => random.Next(1, maxPage + 1);

        private async void Worker()
        {

            int pageId = Settings.PageId;

            if (Settings.Section == "random")
                pageId = GetPageId(Settings.MaxPageId);

            string recipeName;
            recipeName = Settings.RecipeName;

            string source;            
            source = await loader.GetSource(pageId, recipeName);
            
            var domParser = new HtmlParser();
            
            var document = await domParser.ParseDocumentAsync(source);

            var result = parser.Parse(document);
            
            OnNewData?.Invoke(this, result);

        }
    }
}