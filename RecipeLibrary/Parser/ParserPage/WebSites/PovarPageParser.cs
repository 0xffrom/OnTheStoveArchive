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
        private List<RecipeShort> listRecipes = new List<RecipeShort>();
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var recipesBody = document.QuerySelectorAll("div")
                .Where(item => item.ClassName != null && item.ClassName == "recipe"
                                                      && item.ParentElement != null &&
                                                      item.ParentElement.ClassName.Contains("recipe_list")).ToArray();

            foreach (var recipe in recipesBody)
            {
                var anyBody = recipe.QuerySelector("h3").QuerySelector("a");
                string name = anyBody.TextContent;
                string url = "https://povar.ru" + anyBody.Attributes[0].Value;

                var pictureBody = recipe.QuerySelector("img");
                string pictureUrl = pictureBody.Attributes[0].Value;

                var recipeShort = new RecipeShort(name, new Picture(pictureUrl), url);
                
                listRecipes.Add(recipeShort);
            }
            
            return listRecipes.ToArray();
        }
    }
}