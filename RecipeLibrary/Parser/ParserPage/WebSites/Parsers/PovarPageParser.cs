using AngleSharp.Html.Dom;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserPage.Core;
using System.Linq;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    internal class PovarPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            var recipesBody = document.QuerySelectorAll("div.recipe_list > div.recipe");

            double indexStartPopularity = settings.IndexPopularity;

            return (from recipe in recipesBody
                let anyBody = recipe.QuerySelector("h3 > a")
                let title = anyBody.TextContent
                let url = "https://povar.ru" + anyBody.Attributes[0].Value
                let pictureBody = recipe.QuerySelector("img")
                let imageUrl = pictureBody.Attributes[0].Value
                let indexPopularity = indexStartPopularity -= settings.IndexStep
                select new RecipeShort(title, new Image(imageUrl), url, indexPopularity)).ToArray();
        }
    }
}