using AngleSharp.Html.Dom;

namespace RecipeLibrary.Parser.ParserPage.Core
{
    internal interface IParserPage<out T> where T : class
    {
        T Parse(IHtmlDocument document, IParserPageSettings settings);
    }
}
