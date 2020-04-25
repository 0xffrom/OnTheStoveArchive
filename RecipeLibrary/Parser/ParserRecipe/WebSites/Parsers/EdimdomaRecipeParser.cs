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

            var recipeBody = document.QuerySelector("div[class='grid-three-column__column grid-three-column__column_center onthe_data']") 
                ?? throw new ParserException("Не найдено главное тело рецепта.", "edimdoma.ru");

            Title = recipeBody.Attributes[5].Value;

            TitleImage = new Image(recipeBody.QuerySelector("img").Attributes[2].Value);

            Description = recipeBody.QuerySelector("div[class='recipe_description']").TextContent;

            var ingredientBody = recipeBody.QuerySelector("div[id='recipe_ingredients_block']");
            var inputArray = ingredientBody?.QuerySelectorAll("input[class='checkbox__input recipe_ingredient_checkbox']");

            if (inputArray != null)
            {
                List<Ingredient> ingredients = new List<Ingredient>(inputArray.Length);

                foreach (var input in inputArray)
                {
                    string titleIngredient = input.Attributes[2].Value;
                    string name = input.Attributes[4].Value;
                    string unit = input.Attributes[1].Value + ' ' + input.Attributes[6].Value;

                    name = titleIngredient == "Основные" ? name : name + " (" + titleIngredient + ')';
                    ingredients.Add(new Ingredient(name, unit));
                }

                Ingredients = ingredients.ToArray();
            }

            var stepsBody = recipeBody.QuerySelector("div[class='recipe_steps']") ??
                throw new ParserException("Не найден блок с шагами.", "edimdoma.ru");

            var recipeArray = stepsBody.QuerySelectorAll("div[class='content-box recipe_step']");

            if (recipeArray != null)
            {
                List<StepRecipe> stepsRecipe = new List<StepRecipe>(recipeArray.Length);
                
                stepsRecipe.AddRange(from stepBlock in recipeArray 
                    let stepImage = new Image("https://www.edimdoma.ru" + stepBlock.QuerySelector("img")?.Attributes[0]?.Value ?? 
                                              "/assets/default/recipe_steps/ed4_thumb-2c862fbcf2e544709c77a80ead4a3f58cd9a80e6b65f0ad18839af30ec9a2a5a.png") 
                    let stepDescription = stepBlock.QuerySelector("div[class='plain-text recipe_step_text']")?.TextContent 
                    select new StepRecipe(stepDescription, stepImage));

                StepsRecipe = stepsRecipe.ToArray();
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