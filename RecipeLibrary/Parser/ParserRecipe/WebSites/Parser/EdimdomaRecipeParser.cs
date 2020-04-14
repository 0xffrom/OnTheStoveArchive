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
            #region MainInfo
            var recipeBody = document.QuerySelectorAll("div").FirstOrDefault(element =>
            element.ClassName == "grid-three-column__column grid-three-column__column_center onthe_data") ??
            throw new ParserException("Не найдено главное тело рецепта.", "edimdoma.ru");

            var divs = recipeBody.QuerySelectorAll("div") ??
                throw new ParserException("Не найдены теги 'div' в теле рецепта.", "edimdoma.ru");

            Title = recipeBody.Attributes[5].Value;

            TitleImage = new Image(recipeBody.QuerySelector("img").Attributes[2].Value);

            Description = divs.FirstOrDefault(element => element.ClassName == "recipe_description").TextContent;
            #endregion
            #region IngredientsRecipe
            var ingredientBody = divs.FirstOrDefault(element => element.Attributes[0] != null && element.Attributes[0].Value == "recipe_ingredients_block");

            var inputArray = ingredientBody.QuerySelectorAll("input")
                .Where(element => element.ClassName == "checkbox__input recipe_ingredient_checkbox")
                .ToArray();

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
            #endregion
            #region StepsRecipe
            var stepsBody = divs.FirstOrDefault(element => element.ClassName == "recipe_steps") ??
                throw new ParserException("Не найден блок с шагами.", "edimdoma.ru");

            var recipeArray = stepsBody.QuerySelectorAll("div")
                .Where(element => element.ClassName == "content-box recipe_step")
                .ToArray();

            List<StepRecipe> stepsRecipe = new List<StepRecipe>(recipeArray.Length);

            foreach (var stepBlock in recipeArray)
            {
                Image stepImage = new Image("https://www.edimdoma.ru" + stepBlock.QuerySelector("img").Attributes[5].Value);
                string stepDescription = stepBlock
                    .QuerySelectorAll("div")
                    .Where(element => element.ClassName == "plain-text recipe_step_text")
                    .FirstOrDefault()
                    .TextContent;

                stepsRecipe.Add(new StepRecipe(stepDescription, stepImage));
            }

            StepsRecipe = stepsRecipe.ToArray();
            #endregion
            #region Additional
            string authorName = divs.FirstOrDefault(element => element.ClassName == "person__name").TextContent;

            int.TryParse(divs.FirstOrDefault(element => element.ClassName == "field__container")
                .FirstElementChild.Attributes[3].Value, out int countPortions);

            double prepMinutes = ConvertToMinutes(divs.FirstOrDefault(element => element.ClassName == "entry-stats__value").TextContent);

            #region CPFC
            var cpfcDiv = divs.FirstOrDefault(element => element.ClassName == "nutritional-value__leftside");

            double.TryParse(cpfcDiv.QuerySelectorAll("div")
                .FirstOrDefault(element => element.ClassName == "kkal-meter__value").TextContent, out double calories);

            var tablePFC = cpfcDiv.QuerySelectorAll("div")
                .FirstOrDefault(element => element.ClassName == "nutritional-value__nutritional-list")
                .QuerySelectorAll("table");

            double.TryParse(tablePFC[0].QuerySelectorAll("td")
                .FirstOrDefault(element => element.ClassName == "definition-list-table__td definition-list-table__td_value")
                .TextContent.Replace(" г", string.Empty), out double protein);

            double.TryParse(tablePFC[1].QuerySelectorAll("td")
                .FirstOrDefault(element => element.ClassName == "definition-list-table__td definition-list-table__td_value")
                .TextContent.Replace(" г", string.Empty), out double fats);

            double.TryParse(tablePFC[2].QuerySelectorAll("td")
                .FirstOrDefault(element => element.ClassName == "definition-list-table__td definition-list-table__td_value")
                .TextContent.Replace(" г", string.Empty), out double carbohydrates);
            #endregion
            Additional = new Additional(authorName, countPortions, prepMinutes, new CPFC(calories, protein, fats, carbohydrates));
            #endregion

            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients,
                StepsRecipe,
                Additional);
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