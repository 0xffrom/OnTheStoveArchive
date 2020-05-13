using AngleSharp.Html.Parser;
using System;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserRecipe.Core
{
    internal class ParserRecipe<T> where T : RecipeFull
    {
        private IParserRecipeSettings parserSettings;
        private HtmlLoaderRecipe loader;
        private IParserRecipe<T> Parser { get; }

        private IParserRecipeSettings Settings
        {
            get => parserSettings;
            set
            {
                parserSettings = value;
                loader = new HtmlLoaderRecipe(value);
            }
        }

        private ParserRecipe(IParserRecipe<T> parser)
        {
            Parser = parser;
        }
        public ParserRecipe(IParserRecipe<T> parser, IParserRecipeSettings parserSettings) : this(parser)
        {
            Settings = parserSettings;
        }

        internal async Task<T> Worker()
        {
            try
            {
                var source = await loader.GetSource(Settings.Url);

                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);

                var result = Parser.Parse(document, parserSettings);

                return result;
            }
            catch (Exception e)
            {
                throw new ParserException("Ошибка при парсинге страницы: " + e);
            }
        }
    }
}