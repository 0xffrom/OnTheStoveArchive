using System;
using System.Linq;
using System.Reflection;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    
    class PovarPageParser : IParserPage<RecipeShort[]>
    {
        
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            try
            {
                var recipesBody = document.QuerySelectorAll("div")
                    .Where(item => item.ClassName != null && item.ClassName == "recipe"
                                                          && item.ParentElement != null &&
                                                          item.ParentElement.ClassName == ("recipe_list")).ToArray();
                
                double indexStartPopularity = settings.IndexPopularity;
                
                return (from recipe in recipesBody
                    let anyBody = recipe.QuerySelector("h3").QuerySelector("a")
                    let title = anyBody.TextContent
                    let url = "https://povar.ru" + anyBody.Attributes[0].Value
                    let pictureBody = recipe.QuerySelector("img")
                    let pictureUrl = pictureBody.Attributes[0].Value
                    let indexPopularity = indexStartPopularity -= settings.IndexStep
                    select new RecipeShort(title, new Picture(pictureUrl), url,indexPopularity)).ToArray();
            }
            catch (Exception exp)
            {
                throw  new ParserException(exp.Message, this.ToString());
            }
        }
    }
}