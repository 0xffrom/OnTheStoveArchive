/*
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
            url = document
                .QuerySelectorAll("meta")
                .Where(item =>
                item.Attributes[0] != null &&
                item.Attributes[0].Value.Contains("og:url"))
                .Select(item => item.Attributes[1].Value)
                .ToArray()[0];

            title = document
                .QuerySelectorAll("meta")
                .Where(item =>
                item.Attributes[0] != null &&
                item.Attributes[0].Value.Contains("og:title"))
                .Select(item => item.Attributes[1].Value)
                .ToArray()[0];

             titlePicture = document
                .QuerySelectorAll("meta")
                .Where(item => 
                item.Attributes[0] != null &&
                item.Attributes[0].Value == "og:image")
                .Select(item => item.Attributes[1].Value)
                .ToArray()[0];
;
            introductionContent = document
                .QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:description"))
                .Select(item => item.Attributes[1].Value)
                .ToArray()[0];
            
            var ingredientsArray = document
                .QuerySelectorAll("div")
                .Where(item =>
                item.ClassName != null && 
                item.ClassName.Contains("field-row recipe_ingredients"))
                .ToArray();

            List<Ingredient> ingredientsList = new List<Ingredient>();
            for (int i = 0; i < ingredientsArray.Length; i++)
            {
                string titleRecipe;
                titleRecipe = ingredientsArray[i].QuerySelector("div").TextContent ?? title;

                var elementsRecipe = ingredientsArray[i]
                    .QuerySelectorAll("tr")
                    .Where(item =>
                    item.ClassName != null && 
                    item.ClassName.Contains("definition-list-table__tr"))
                    .ToArray();

                for (int j = 0; j < elementsRecipe.Length; j++)
                {
                    string name;
                    string unit;

                    name = elementsRecipe[j]
                        .QuerySelectorAll("span")
                        .Where(item =>
                        item.ClassName != null &&
                        item.ClassName.Contains("recipe_ingredient_title"))
                        .ToArray()[0].TextContent;

                    unit = elementsRecipe[j]
                        .QuerySelectorAll("td").Where(item =>
                        item.ClassName != null &&
                        item.ClassName.Contains("definition-list-table__td definition-list-table__td_value"))
                        .ToArray()[0].TextContent;

                    ingredientsList.Add(new Ingredient(titleRecipe, name, unit));
                }

                ingredients = ingredientsList.ToArray();
            }

            var stepOfRecipes = document
                .QuerySelectorAll("div")
                .Where(item => item.ClassName != null && 
                item.ClassName == "content-box recipe_step")
                .ToArray();

            List<StepRecipe> steps = new List<StepRecipe>();
            for (int i = 0; i < stepOfRecipes.Length; i++)
            {  
                var titles = stepOfRecipes[i]
                    .QuerySelectorAll("div")
                    .Where(item => item.ClassName != null && 
                    item.ClassName.Contains("plain-text recipe_step_text"))
                    .Select(item => item.TextContent)
                    .ToArray();

                
                if (titles.Length != 1)
                    throw new ParserException("Содержится ни только одно описание рецептов");

                string titleRecipe;
                titleRecipe = titles[0];

                string[] urlPicturesRecipe;

                urlPicturesRecipe = stepOfRecipes[i]
                    .QuerySelectorAll("img")
                    .Where(item => 
                    item.ClassName != null &&
                    item.ClassName.Contains("lazy recipe_step_image") && 
                    item.ParentElement.ClassName != null && 
                    item.ParentElement.ClassName.Contains("step-image-container-posted"))
                    .Select(item => "https://www.edimdoma.ru" + item.Attributes[0].Value).ToArray();

                steps.Add(new StepRecipe(titleRecipe, urlPicturesRecipe));

            }

            stepsOfRecipe = steps.ToArray();



            //TODO: добавить видеорецепты, конец рецепта => текст и картинки.x
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
*/