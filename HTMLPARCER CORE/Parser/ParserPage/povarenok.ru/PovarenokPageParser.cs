using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.ParsePage
{
    class PovarenokPageParser : IParserPage<RecipeShort[]>
    {
        private List<RecipeShort> listRecipes = new List<RecipeShort>();
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var recipesList = document.QuerySelectorAll("article")
                .Where(item => item.ClassName != null && item.ClassName.Contains("item-bl"));

            int countRecipes = recipesList.Count();

            foreach (var recipeBlock in recipesList)
            {
                string url = recipeBlock.QuerySelectorAll("div")
                    .Where(item => item.ClassName != null && item.ClassName.Contains("m-img desktop-img conima"))
                    .Select(item => item.FirstElementChild.Attributes[0].Value).ToArray()[0];
                
                string urlPicture = recipeBlock.QuerySelectorAll("div")
                    .Where(item => item.ClassName != null && item.ClassName.Contains("m-img desktop-img conima"))
                    .Select(item => item.FirstElementChild.FirstElementChild.Attributes[0].Value).ToArray()[0];

                Picture picture = new Picture(urlPicture);

                string title = recipeBlock.QuerySelector("h2").QuerySelector("a").TextContent;

                RecipeShort recipeShort = new RecipeShort(title, picture, url);

                listRecipes.Add(recipeShort);
            }

            // TODO: Потестить.


            return listRecipes.ToArray();
        }
    }
}
