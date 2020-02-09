using AngleSharp.Html.Dom;

namespace RecipeLibrary.Parser.ParserPage.Core
{
    internal interface IParserPage<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
