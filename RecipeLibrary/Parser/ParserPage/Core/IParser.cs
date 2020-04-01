using AngleSharp.Html.Dom;

namespace ObjectsLibrary.Parser.ParserPage.Core
{
    internal interface IParserPage<out T> where T : class
    {
        T Parse(IHtmlDocument document, IParserPageSettings settings);
    }
}
