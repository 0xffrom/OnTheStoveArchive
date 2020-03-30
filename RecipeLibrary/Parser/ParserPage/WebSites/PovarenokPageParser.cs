using System;
using System.Linq;
using System.Reflection;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    [assembly: AssemblyTitle("povarenok.ru")]
    class PovarenokPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            try
            {
                var recipesList = document.QuerySelectorAll("article")
                    .Where(item => item.ClassName != null && item.ClassName == ("item-bl"));

                var recipeBlocks = recipesList as IElement[] ?? recipesList.ToArray();

                return (from recipeBlock in recipeBlocks
                    let url = recipeBlock.QuerySelectorAll("div")
                        .Where(item => item.ClassName != null && item.ClassName == ("m-img desktop-img conima"))
                        .Select(item => item.FirstElementChild.Attributes[0].Value)
                        .ToArray()[0]
                    let urlPicture = recipeBlock.QuerySelectorAll("div")
                        .Where(item => item.ClassName != null && item.ClassName == ("m-img desktop-img conima"))
                        .Select(item => item.FirstElementChild.FirstElementChild.Attributes[0].Value)
                        .ToArray()[0]
                    let picture = new Picture(urlPicture)
                    let title = recipeBlock.QuerySelector("h2").QuerySelector("a").TextContent
                    select new RecipeShort(title, picture, url)).ToArray();
            }
            catch (Exception exp)
            {
                throw  new ParserException(exp.Message, this.ToString());
            }
        }
    }
}