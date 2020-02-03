using AngleSharp.Html.Dom;

namespace RecipeLibrary.Parse
{
    internal interface IParserPage<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
