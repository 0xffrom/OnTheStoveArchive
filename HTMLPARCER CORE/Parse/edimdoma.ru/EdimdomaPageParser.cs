using System.Collections.Generic;
using System.Linq;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class EdimdomaPageParser : IParser<RecipeFull[]>
    {
        private string url;
        private string webSite = "edimdoma.ru";
        private string title;
        private string titlePicture;
        private Ingredient[] ingredients;
        private StepRecipe[] stepsOfRecipe;
        private string introductionContent;
        private string endContentText;
        private string[] endContentPictures;
        private string[] contentVideos;

        public RecipeFull[] Parse(IHtmlDocument document)
        {
            url = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:url")).ToArray()[0]
                .Attributes[1].Value;

            title = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:title")).ToArray()[0]
                .Attributes[1].Value;

             titlePicture = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value == "og:image").ToArray()[0]
                .Attributes[1].Value;
;
            introductionContent = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:description"))
                .ToArray()[0]
                .Attributes[1].Value;
            
            var ingredientsArray = document.QuerySelectorAll("div").Where(item =>
                item.ClassName != null && item.ClassName.Contains("field-row recipe_ingredients")).ToArray();

            List<Ingredient> ingredientsList = new List<Ingredient>();
            for (int i = 0; i < ingredientsArray.Length; i++)
            {
                string titleRecipe;
                titleRecipe = ingredientsArray[i].QuerySelector("div").TextContent ?? title;
                // name + unit
                var elementsRecipe = ingredientsArray[i].QuerySelectorAll("tr").Where(item =>
                    item.ClassName != null && item.ClassName.Contains("definition-list-table__tr")).ToArray();

                for (int j = 0; j < elementsRecipe.Length; j++)
                {
                    string name;
                    string unit;

                    name = elementsRecipe[j].QuerySelectorAll("span").Where(item =>
                            item.ClassName != null &&
                            item.ClassName.Contains("recipe_ingredient_title"))
                        .ToArray()[0].TextContent;

                    unit = elementsRecipe[j].QuerySelectorAll("td").Where(item =>
                            item.ClassName != null &&
                            item.ClassName.Contains("definition-list-table__td definition-list-table__td_value"))
                        .ToArray()[0].TextContent;
                    System.Console.WriteLine($"{titleRecipe} {name} {unit}");
                    ingredientsList.Add(new Ingredient(titleRecipe, name, unit));
                }

                ingredients = ingredientsList.ToArray();
            }
            
            //TODO: доделать рецепты, добавить видеорецепты, конец рецепта => текст и картинки.x
            return new RecipeFull[]
            {
                new RecipeFull(url, webSite, title, titlePicture,
                    ingredients, stepsOfRecipe, introductionContent,
                    endContentText, endContentPictures, contentVideos)
            };
        }

        public int GetCount() => 1;

  
    }
}