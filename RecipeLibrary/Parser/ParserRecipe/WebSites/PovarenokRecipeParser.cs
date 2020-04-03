using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarenokRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Image TitlePicture { get; set; }
        private string Description { get; set; }
        private Ingredient[] Ingredients { get; set; }
        private StepRecipe[] StepRecipesBoxes { get; set; }
        private Additional Additional { get; set; }

        private const string WhiteSpaceBug = "  ";

        public RecipeFull Parse(IHtmlDocument document)
        {
            var recipeBody = document
                .QuerySelectorAll("article")
                .FirstOrDefault(element => element.ClassName != null && element.ClassName == "item-bl item-about");

            // recipeBody =>  главный фрейм с рецептом, если он существует - работаем с ним, иначе - рецепта не сущесвует.
            if (recipeBody == null)
                return new RecipeFull();


            #region Title

            Title = recipeBody.QuerySelector("h1").TextContent;

            TitlePicture = new Image(recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "m-img")
                .Select(element => element.FirstElementChild?.Attributes[1]?.Value)
                .FirstOrDefault());

            #endregion

            #region Description

            Description = document.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "article-text")
                .Select(element => element.TextContent).ToArray()[0]
                .Replace("\n", String.Empty)
                .Replace("  ", String.Empty);

            #endregion

            #region IngredientBox

            var ingredientBody = recipeBody
                .QuerySelectorAll("div")
                .FirstOrDefault(element => element.ClassName != null && element.ClassName == "ingredients-bl");

            int countIngredientTitles = ingredientBody?.QuerySelectorAll("ul").Length ?? 0;

            var ingredientsList = new List<Ingredient>();

            for (int i = 0; i < countIngredientTitles; i++)
            {
                // "Время приготовления" и "Количество порций".
                int count = 0;

                var p = ingredientBody?.QuerySelectorAll("p")
                    .Select(item => item.TextContent).ToArray();

                if (p?.Length != 0)
                {
                    if (p.Any(element => element.Contains("Время приготовления:")))
                        count++;
                    if (p.Any(element => element.Contains("Количество порций:")))
                        count++;
                }


                string titleIngredient;

                var titleBody = ingredientBody?.QuerySelectorAll("p").ToArray();
                if (titleBody?.Length - count == 0)
                    titleIngredient = Title;

                else
                    titleIngredient = ingredientBody?.QuerySelectorAll("p")
                        .Select(item => item.TextContent).ToArray()[i];


                var ingredientsArray = ingredientBody?.QuerySelectorAll("ul")
                    .ToArray()[i]
                    .QuerySelectorAll("li")
                    .ToArray();

                if (ingredientsArray != null)
                {
                    Ingredient[] ingredients = new Ingredient[ingredientsArray.Length];

                    for (int j = 0; j < ingredientsArray.Length; j++)
                    {
                        string name = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                                item.Attributes[0] != null && item.Attributes[0].Value == ("name"))
                            .Select(item => item.TextContent).FirstOrDefault();

                        string unit = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                                item.Attributes[0] != null && item.Attributes[0].Value == ("amount"))
                            .Select(item => item.TextContent).FirstOrDefault();

                        name += ingredientsArray[j].TextContent.Replace(name ?? "А тут может и ничего не быть.", string.Empty)
                            .Replace(unit ?? "А тут может и ничего не быть.", string.Empty)
                            .Replace("\n", string.Empty)
                            .Replace(WhiteSpaceBug, string.Empty)
                            .Replace("—", string.Empty);

                        if (titleIngredient != Title)
                            name += $" ({titleIngredient})";
                    
                        Ingredient ingredient = new Ingredient(name, unit);
                        ingredients[j] = ingredient;
                    }

                    ingredientsList.AddRange(ingredients);
                }
            }

            Ingredients = ingredientsList.ToArray();

            #endregion

            #region StepRecipeBox

            var recipesArray = recipeBody.QuerySelectorAll("div")
                .Where(item => item.ClassName != null && item.ClassName == ("cooking-bl"))
                .ToArray();

            int countRecipes = recipesArray.Length;

            StepRecipe[] stepRecipeBoxes = new StepRecipe[countRecipes];

            for (int i = 0; i < countRecipes; i++)
            {
                string imageUrl = recipesArray[i]?.FirstElementChild?.FirstElementChild?.Attributes[2]?.Value;
                string description = recipesArray[i]?.LastElementChild?.FirstElementChild?.TextContent;

                var image = new Image(imageUrl);

                var stepRecipeBox = new StepRecipe(description, image);

                stepRecipeBoxes[i] = stepRecipeBox;
            }

            StepRecipesBoxes = stepRecipeBoxes;

            #endregion

            #region Additional

            // author name
            // КБЖУ
            // оличество минут
            // Количество порций


            #endregion


            return new RecipeFull(string.Empty, Title, TitlePicture, Description, Ingredients,
                StepRecipesBoxes,
                Additional);
            ;
        }
    }
}