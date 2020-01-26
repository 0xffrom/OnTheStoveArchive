using AngleSharp.Html.Dom;

namespace RecipeLibrary.Parse
{
    internal interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
