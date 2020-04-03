using System;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    class PovarenokPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            try
            {
                var recipesList = document.QuerySelectorAll("article")
                    .Where(item => item.ClassName != null && item.ClassName == ("item-bl"));

                var recipeBlocks = recipesList as IElement[] ?? recipesList.ToArray();
                
                double indexStartPopularity = settings.IndexPopularity;
                
                return (from recipeBlock in recipeBlocks
                    let url = recipeBlock.QuerySelectorAll("div")
                        .Where(item => item.ClassName != null && item.ClassName == ("m-img desktop-img conima"))
                        .Select(item => item.FirstElementChild.Attributes[0].Value)
                        .ToArray()[0]
                    let urlPicture = recipeBlock.QuerySelectorAll("div")
                        .Where(item => item.ClassName != null && item.ClassName == ("m-img desktop-img conima"))
                        .Select(item => item.FirstElementChild.FirstElementChild.Attributes[0].Value)
                        .ToArray()[0]
                    let picture = new Image(urlPicture)
                    let image = recipeBlock.QuerySelector("h2").QuerySelector("a").TextContent
                    let indexPopularity = indexStartPopularity -= settings.IndexStep
                    select new RecipeShort(image, picture, url, indexStartPopularity)).ToArray();
            }
            catch (Exception exp)
            {
                throw  new ParserException(exp.Message, this.ToString());
            }
        }
    }
}