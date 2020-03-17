using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    class PovarPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var recipesBody = document.QuerySelectorAll("div")
                .Where(item => item.ClassName != null && item.ClassName == "recipe"
                                                      && item.ParentElement != null &&
                                                      item.ParentElement.ClassName == ("recipe_list")).ToArray();

            return (from recipe in recipesBody
                let anyBody = recipe.QuerySelector("h3").QuerySelector("a")
                let name = anyBody.TextContent
                let url = "https://povar.ru" + anyBody.Attributes[0].Value
                let pictureBody = recipe.QuerySelector("img")
                let pictureUrl = pictureBody.Attributes[0].Value
                select new RecipeShort(name, new Picture(pictureUrl), url)).ToArray();
        }
    }
}