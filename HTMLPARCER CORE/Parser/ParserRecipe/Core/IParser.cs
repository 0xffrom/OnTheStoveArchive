using AngleSharp.Html.Dom;

namespace RecipeLibrary.Parser.ParserRecipe.Core
{
    internal interface IParserRecipe<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}