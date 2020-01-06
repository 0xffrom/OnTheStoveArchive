using AngleSharp.Html.Dom;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParserRecipe
    {
        T Parse(IHtmlDocument document);
        
    }
}