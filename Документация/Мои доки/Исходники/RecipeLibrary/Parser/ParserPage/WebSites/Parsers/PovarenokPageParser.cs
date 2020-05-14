using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserPage.Core;
using System.Linq;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    internal class PovarenokPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            var recipesList = document.QuerySelectorAll("article").Where(x=> x.ClassName == "item-bl");

            double indexStartPopularity = settings.IndexPopularity;

            return (from recipeBlock in recipesList
                let url = recipeBlock.QuerySelector("div.m-img.desktop-img.conima")
                    .FirstElementChild.Attributes[0].Value
                let image = new Image(recipeBlock.QuerySelector("div.m-img.desktop-img.conima")
                    .FirstElementChild.FirstElementChild.Attributes[0].Value)
                let title = recipeBlock.QuerySelector("h2").QuerySelector("a").TextContent
                let indexPopularity = indexStartPopularity -= settings.IndexStep
                select new RecipeShort(title, image, url, indexStartPopularity)).ToArray();
        }
    }
}