using AngleSharp.Html.Dom;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParserRecipe<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}