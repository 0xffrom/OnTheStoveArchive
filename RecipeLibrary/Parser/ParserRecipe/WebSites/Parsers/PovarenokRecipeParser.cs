using AngleSharp.Html.Dom;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarenokRecipeParser : IParserRecipe<RecipeFull>
    {

        /// <see cref="RecipeFull.Url"/>
        private string Url { get; set; }

        /// <see cref="RecipeFull.Title"/>
        private string Title { get; set; }

        /// <see cref="RecipeFull.TitleImage"/>
        private Image TitleImage { get; set; }

        /// <see cref="RecipeFull.Description"/>
        private string Description { get; set; }

        /// <see cref="RecipeFull.Ingredients"/>
        private Ingredient[] Ingredients { get; set; }

        /// <see cref="RecipeFull.StepsRecipe"/>
        private StepRecipe[] StepsRecipe { get; set; }

        /// <see cref="RecipeFull.Additional"/>
        private Additional Additional { get; set; }

        private const string WhiteSpaceBug = "  ";


        /// <see cref="IParserRecipe{T}.Parse(IHtmlDocument, IParserRecipeSettings)"/>
        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            var recipeBody = document.QuerySelector("article.item-bl.item-about");
            Url = parserRecipeSettings.Url;
            
            Title = recipeBody.QuerySelector("h1").TextContent;
            TitleImage = new Image(recipeBody.QuerySelector("div.m-img > div:first-child")?.Attributes[1]?.Value);
            
            Description = recipeBody.QuerySelector("div.article-text")
                .TextContent
                .Replace("\n", String.Empty)
                .Replace("  ", String.Empty);

            var ingredientBody = recipeBody.QuerySelector("div.ingredients-bl");
            int countIngredientTitles = ingredientBody.QuerySelectorAll("ul").Length;
            var ingredientsList = new List<Ingredient>();

            for (int i = 0; i < countIngredientTitles; i++)
            {
                // "Время приготовления" и "Количество порций".
                int count = 0;

                var p = ingredientBody?.QuerySelectorAll("p")
                    .Select(item => item.TextContent).ToArray();

                if (p.Length != 0)
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
                        string name = ingredientsArray[j].QuerySelector("span[itemprop='name']")?.TextContent;

                        string unit = ingredientsArray[j].QuerySelector("span[itemprop='amount']")?.TextContent;

                        name += ingredientsArray[j].TextContent
                            .Replace(name ?? "old_value", string.Empty)
                            .Replace(unit ?? "old_value", string.Empty)
                            .Replace("\n", string.Empty)
                            .Replace(WhiteSpaceBug, string.Empty)
                            .Replace("—", string.Empty);

                        if (titleIngredient != Title)
                            name += $" ({titleIngredient})";

                        Ingredient ingredient = new Ingredient(name, unit, Title);
                        ingredients[j] = ingredient;
                    }

                    ingredientsList.AddRange(ingredients);
                }
            }

            Ingredients = ingredientsList.ToArray();
            
            var recipesArray = recipeBody.QuerySelectorAll("div.cooking-bl");
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

            StepsRecipe = stepRecipeBoxes;

            string authorName = recipeBody.QuerySelector("a[title='Профиль пользователя']")?.TextContent;
            var ingredientBodyP = ingredientBody.QuerySelectorAll("p");

            double prepMinutes = ConvertToMinutes(ingredientBodyP
                .Where(x => x.FirstElementChild.TextContent.Contains("Время приготовления"))
                .Select(x => x.LastElementChild.TextContent).FirstOrDefault());

            int countPortions = int.Parse(ingredientBodyP
                .Where(x => x.FirstElementChild.TextContent.Contains("Количество порций:"))
                .Select(x => x.TextContent).FirstOrDefault()?.Replace("Количество порций:", string.Empty) ?? "0");

            Additional = new Additional(authorName, countPortions, prepMinutes, new CPFC());

            var tableBody = recipeBody.QuerySelector("div[id='nae-value-bl']");
            if (tableBody != null)
            {
                var tableCPFC = tableBody
                    .LastElementChild?
                    .FirstElementChild?
                    .QuerySelectorAll("tr")[3]
                    .QuerySelectorAll("strong")
                    .Select(x => x.TextContent)
                    .ToArray();
                
                CPFC CPFC = null;

                if (tableCPFC != null)
                {
                    double calories = double.Parse(tableCPFC[0].Replace(" ккал", string.Empty).Replace('.', ','));
                    double protein = double.Parse(tableCPFC[1].Replace(" г", string.Empty).Replace('.', ','));
                    double fats = double.Parse(tableCPFC[2].Replace(" г", string.Empty).Replace('.', ','));
                    double carbohydrates = double.Parse(tableCPFC[3].Replace(" г", string.Empty).Replace('.', ','));

                    CPFC = new CPFC(calories, protein, fats, carbohydrates);
                }


                Additional = new Additional(authorName, countPortions, prepMinutes, CPFC);
            }


            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients, StepsRecipe, Additional);
        }

        /// <see cref="IParserRecipe{T}.ConvertToMinutes(string)"/>
        public double ConvertToMinutes(string inputLine)
        {
            if (inputLine == null)
                return 0;

            // 40 минут, 80 минут, 10 минут.

            inputLine = inputLine.Replace(" и", String.Empty);

            string[] arrayWords = inputLine.Split(' ');

            double minutes = 0;


            for (int i = 1; i < arrayWords.Length; i += 2)
            {
                if (arrayWords[i].Contains('м'))
                    minutes += int.Parse(arrayWords[i - 1]);
                else if (arrayWords[i].Contains('ч'))
                    minutes += int.Parse(arrayWords[i - 1]) * 60;
                else if (arrayWords[i].Contains('д'))
                    // 60 * 24 = 1440‬
                    minutes += int.Parse(arrayWords[i - 1]) * 1440;
            }

            return minutes;
        }
    }
}