using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimdomaRecipeParser : IParserRecipe<RecipeFull>
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

        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            Url = parserRecipeSettings.Url;

            var recipeBody = document.QuerySelector("div.grid-three-column__column.grid-three-column__column_center.onthe_data") 
                ?? throw new ParserException("Не найдено главное тело рецепта.", "edimdoma.ru");

            Title = recipeBody.Attributes[5].Value ?? string.Empty;

            TitleImage = new Image(recipeBody.QuerySelector("img").Attributes[2].Value ?? string.Empty);

            Description = recipeBody.QuerySelector("div.recipe_description")?.TextContent ?? string.Empty;

            var ingredientBody = recipeBody.QuerySelector("div[id='recipe_ingredients_block']");
            var inputArray = ingredientBody?.QuerySelectorAll("input.checkbox__input.recipe_ingredient_checkbox");

            if (inputArray != null)
            {
                List<Ingredient> ingredients = new List<Ingredient>(inputArray.Length);

                foreach (var input in inputArray)
                    
                {
                    string titleIngredient = input.Attributes[2].Value;
                    string name = input.Attributes[4].Value;
                    string unit = input.Attributes[1].Value + ' ' + input.Attributes[6].Value;

                    name = titleIngredient == "Основные" ? name : name + " (" + titleIngredient + ')';
                    ingredients.Add(new Ingredient(name, unit, Title));
                }

                Ingredients = ingredients.ToArray();
            }

            var stepsBody = recipeBody.QuerySelector("div.recipe_steps") ??
                throw new ParserException("Не найден блок с шагами.", "edimdoma.ru");

            var recipeArray = stepsBody.QuerySelectorAll("div.content-box.recipe_step");

            if (recipeArray != null)
            {
                List<StepRecipe> stepsRecipe = new List<StepRecipe>(recipeArray.Length);
                
                stepsRecipe.AddRange(from stepBlock in recipeArray 
                    let stepImage = new Image("https://www.edimdoma.ru" + stepBlock.QuerySelector("img")?.Attributes[0]?.Value ?? 
                                              "/assets/default/recipe_steps/ed4_thumb-2c862fbcf2e544709c77a80ead4a3f58cd9a80e6b65f0ad18839af30ec9a2a5a.png") 
                    let stepDescription = stepBlock.QuerySelector("div.plain-text.recipe_step_text")?.TextContent 
                    select new StepRecipe(stepDescription, stepImage));

                StepsRecipe = stepsRecipe.ToArray();
            }

            string authorName = recipeBody.QuerySelector("div.person__name").TextContent;

                int.TryParse(recipeBody.QuerySelector("div.field__container")?.FirstElementChild?.Attributes[3].Value ?? "0", out int countPortions);

                double prepMinutes = ConvertToMinutes(recipeBody.QuerySelector("div.entry-stats__value").TextContent);

                var cpfcDiv = recipeBody.QuerySelector("div.nutritional-value__leftside");

            if (cpfcDiv != null)
            {
                double.TryParse(cpfcDiv.QuerySelector("div.kkal-meter__value")?.TextContent ?? "0", out double calories);

                var tablePFC = cpfcDiv.QuerySelectorAll("div.nutritional-value__nutritional-list > table");

                if (tablePFC != null)
                {
                    double.TryParse(
                        tablePFC[0].QuerySelector("td.definition-list-table__td.definition-list-table__td_value")
                            .TextContent.Replace(" г", string.Empty) ?? "0", out double protein);

                    double.TryParse(
                        tablePFC[1].QuerySelector("td.definition-list-table__td.definition-list-table__td_value")
                            .TextContent.Replace(" г", string.Empty) ?? "0", out double fats);

                    double.TryParse(
                        tablePFC[2].QuerySelector("td.definition-list-table__td.definition-list-table__td_value")
                            .TextContent.Replace(" г", string.Empty) ?? "0", out double carbohydrates);

                    Additional = new Additional(authorName, countPortions, prepMinutes,
                        new CPFC(calories, protein, fats, carbohydrates));
                }
                else
                    Additional = new Additional(authorName, countPortions, prepMinutes, new CPFC());
            }

            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients, StepsRecipe, Additional);
        }

        public double ConvertToMinutes(string inputLine)
        {
            if (inputLine is null)
                return 0;

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