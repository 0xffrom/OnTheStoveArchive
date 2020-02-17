using System;
using AngleSharp.Html.Parser;

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
            Parser = parser;
        }

        public ParserRecipe(IParserRecipe<T> parser, IParserRecipeSettings parserSettings) : this(parser)
        {
            Settings = parserSettings;
        }


        internal event Action<object, T> OnNewData;

        internal void StartParseRecipe() => Worker();

        private async void Worker()
        {
            try
            {
                var source = await loader.GetSource(Settings.Url);

                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);

                var result = parser.Parse(document);

                OnNewData?.Invoke(this, result);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception. Message: {e.Message}. Source: {e.Source}");
                OnNewData?.Invoke(this, null);
            }

        }
    }
}