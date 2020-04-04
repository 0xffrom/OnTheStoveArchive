using AngleSharp.Html.Parser;
using System;
using System.Threading.Tasks;

namespace ObjectsLibrary.Parser.ParserRecipe.Core
{
    internal class ParserRecipe<T> where T : RecipeFull
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

        internal async Task<T> Worker()
        {
            try
            {
                var source = await loader.GetSource(Settings.Url);

                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);

                var result = parser.Parse(document, parserSettings);

                return result;

            }
            catch (Exception e)
            {
                throw new ParserException("Ошибка при парсинге страницы: " + e);
            }

        }
    }
}