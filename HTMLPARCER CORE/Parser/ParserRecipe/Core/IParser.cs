using AngleSharp.Html.Dom;

namespace RecipeLibrary.ParseRecipe
{
    internal interface IParserRecipe<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}