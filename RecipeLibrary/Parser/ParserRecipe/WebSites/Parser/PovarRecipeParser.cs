using AngleSharp.Html.Dom;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarRecipeParser : IParserRecipe<RecipeFull>
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

        /// <see cref="RecipeFull.StepRecipesBoxes"/>
        private StepRecipe[] StepRecipesBoxes { get; set; }

        /// <see cref="RecipeFull.Additional"/>
        private Additional Additional { get; set; }

        /// <see cref="IParserRecipe{T}.Parse(IHtmlDocument, IParserRecipeSettings)"/>
        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            Url = parserRecipeSettings.Url;

            var recipeBody = document
                .QuerySelectorAll("div")
                .FirstOrDefault(el => el.ClassName != null && el.ClassName == "cont_area");

            if (recipeBody is null)
                return new RecipeFull();

            #region Title

            Title = recipeBody.QuerySelectorAll("h1")
                .Where(el => el.ClassName != null
                             && el.ClassName == "detailed"
                             && el.Attributes[1] != null
                             && el.Attributes[1].Value == "name")
                .Select(el => el.TextContent)
                .FirstOrDefault();

            TitleImage = new Image(recipeBody
                .QuerySelectorAll("div")
                .FirstOrDefault(el => el.ClassName != null && el.ClassName == "bigImgBox")
                ?.QuerySelector("a")
                .FirstElementChild.Attributes[0].Value);

            #endregion

            #region Description

            Description = String.Empty;

            foreach (var textLine in recipeBody
                .QuerySelectorAll("span")
                .Where(el => el.ClassName != null && el.ClassName == "detailed_full")
                .Select(x => x.TextContent))
            {
                Description += Environment.NewLine + textLine;
            }
            #endregion

            #region Ingredients

            var ingredientsBody = recipeBody
                .QuerySelectorAll("ul")
                .First(x => x.ClassName != null && x.ClassName == "detailed_ingredients")
                .QuerySelectorAll("li")
                .ToArray();

            int countIngredients = ingredientsBody.Length;

            Ingredient[] ingredients = new Ingredient[countIngredients];

            for (int i = 0; i < countIngredients; i++)
            {
                string title = ingredientsBody[i].Attributes[1].Value;
                string unit = ingredientsBody[i].TextContent
                    .Replace(title, string.Empty)
                    .Remove(0, 35);

                ingredients[i] = new Ingredient(title, unit);
            }

            Ingredients = ingredients;

            #endregion

            #region StepRecipes

            var stepRecipeBody = recipeBody
                .QuerySelectorAll("div")
                .FirstOrDefault(el => el.Attributes.Length == 3 && el.Attributes[0].Value == "recipeInstructions");

            var stepCollection = stepRecipeBody
                .QuerySelectorAll("div")
                .Where(x => x.ClassName != null)
                .ToArray();

            List<StepRecipe> stepRecipesBoxes = new List<StepRecipe>(stepCollection.Length / 3);

            foreach (var step in stepCollection)
            {
                switch (step.ClassName)
                {
                    case "detailed_step_photo_big":
                    {
                        var firstEl = step.FirstElementChild;

                        string description = firstEl.Attributes[0].Value;

                        string pictureUrl = firstEl.Attributes[3].Value;

                        stepRecipesBoxes.Add(new StepRecipe(description,
                            new Image(pictureUrl)));
                        break;
                    }
                    case "detailed_step_description_big noPhotoStep":
                    {
                        string description = step.TextContent;
                        stepRecipesBoxes.Add(new StepRecipe(description,
                            new Image()));
                        break;
                    }
                }
            }

            StepRecipesBoxes = stepRecipesBoxes.ToArray();

            #endregion

            #region Additional

            var rcpAuthorTimeBody = recipeBody
                .QuerySelectorAll("div")
                .FirstOrDefault(x => x.ClassName != null && x.ClassName == "rcpAuthorTime");

            string authorName = rcpAuthorTimeBody
                .QuerySelectorAll("span")
                .FirstOrDefault(x => x.Attributes[0] != null && x.Attributes[0].Value == "autorName")
                .TextContent;

            double prepMinutes = ConvertToMinutes(
                rcpAuthorTimeBody
                .QuerySelectorAll("meta")
                .FirstOrDefault(x => x.Attributes[0] != null && x.Attributes[0].Value == "cookTime")
                .Attributes[1].Value);

            string[] portions = recipeBody
                .QuerySelectorAll("em")
                .FirstOrDefault(x => x.Attributes[0] != null && x.Attributes[0].Value == "recipeYield")
                .TextContent
                .Remove(0, 19)
                .Split('-') ?? null;

            int countPortions = 0;

            if (portions != null)
                countPortions = int.Parse(portions[0]);
            
            // На повар.ру нет информации(
            CPFC CPFC = null;

            Additional = new Additional(authorName, countPortions, prepMinutes, CPFC);



            #endregion

            return new RecipeFull(string.Empty, Title, TitleImage, Description, Ingredients,
                StepRecipesBoxes,
                Additional);
        }

        /// <see cref="IParserRecipe{T}.ConvertToMinutes(string)"/>
        public double ConvertToMinutes(string inputLine)
        {
            // Format: PT<min>M
            return double.Parse(inputLine[2..^1]);
        }
    }
}