using AngleSharp.Html.Dom;

namespace RecipeLibrary.ParsePage
{
    internal interface IParserPage<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
