using System;
using AngleSharp.Html.Parser;
using RecipeLibrary.Parse;

namespace RecipeLibrary.Parser.ParserRecipe.Core
{
    internal class ParserRecipe<T> where T : class
    {
        IParserRecipe<T> parser;
        IParserRecipeSettings parserSettings;

        HtmlLoader loader;

        internal IParserRecipe<T> Parser
        {
            get => parser;
            set => parser = value;
        }

        internal IParserRecipeSettings Settings
        {
            get => parserSettings;
            set {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public ParserRecipe(IParserRecipe<T> parser)
        {
            this.parser = parser;
        }

        public ParserRecipe(IParserRecipe<T> parser, IParserRecipeSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }


        internal event Action<object, T> OnNewData;

        internal void StartParseRecipe() => Worker();

        private static readonly Random random = new Random();
        private static int GetPageId(int maxPage) => random.Next(0, maxPage + 1);

        private async void Worker()
        {

            var source = await loader.GetSource();
            
            var domParser = new HtmlParser();
            var document = await domParser.ParseDocumentAsync(source);

            var result = parser.Parse(document);

            OnNewData?.Invoke(this, result);

        }
    }
}