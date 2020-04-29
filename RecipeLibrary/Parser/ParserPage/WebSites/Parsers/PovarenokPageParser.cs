using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserPage.Core;
using System;
using System.Linq;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    class PovarenokPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            var recipesList = document.QuerySelectorAll("article.item-bl");

            double indexStartPopularity = settings.IndexPopularity;

            return (from recipeBlock in recipesList
                    let url = recipeBlock.QuerySelector("div.m-img.desktop-img.conima")
                        .FirstElementChild.Attributes[0].Value
                    let urlPicture = recipeBlock.QuerySelector("div.m-img.desktop-img.conima")
                    .FirstElementChild.FirstElementChild.Attributes[0].Value
                    let picture = new Image(urlPicture)
                    let image = recipeBlock.QuerySelector("h2 > a").TextContent
                    let indexPopularity = indexStartPopularity -= settings.IndexStep
                    select new RecipeShort(image, picture, url, indexStartPopularity)).ToArray();
        }
    }
}