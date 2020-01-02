using AngleSharp.Html.Dom;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
