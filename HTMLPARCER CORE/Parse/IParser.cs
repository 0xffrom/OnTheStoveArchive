using AngleSharp.Html.Dom;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParser<T> where T : class
    {
        RecipeFull[] Parse(IHtmlDocument document);

        int GetCount();
    }
}
