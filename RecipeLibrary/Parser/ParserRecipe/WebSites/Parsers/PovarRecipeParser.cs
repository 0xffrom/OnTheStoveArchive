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

        /// <see cref="RecipeFull.StepsRecipe"/>
        private StepRecipe[] StepsRecipe { get; set; }

        /// <see cref="RecipeFull.Additional"/>
        private Additional Additional { get; set; }

        /// <see cref="IParserRecipe{T}.Parse(IHtmlDocument, IParserRecipeSettings)"/>
        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            Url = parserRecipeSettings.Url;

            var recipeBody = document
                .QuerySelector("div.cont_area");

            if (recipeBody is null)
                return new RecipeFull();


            Title = recipeBody.QuerySelector("h1.detailed[itemprop='name']").TextContent;
            TitleImage = new Image(recipeBody.QuerySelector("div.bigImgBox")?
                .QuerySelector("a").FirstElementChild.Attributes[0].Value);

            Description = String.Empty;

            foreach (var textLine in recipeBody.QuerySelectorAll("span.detailed_full").Select(x => x.TextContent))
                Description += Environment.NewLine + textLine;

            var ingredientsBody = recipeBody.QuerySelectorAll("ul.detailed_ingredients > li");

            int countIngredients = ingredientsBody.Length;

            Ingredient[] ingredients = new Ingredient[countIngredients];

            for (int i = 0; i < countIngredients; i++)
            {
                string title = ingredientsBody[i].Attributes[1].Value;
                string unit = ingredientsBody[i].TextContent
                    .Replace(title, string.Empty)
                    .Remove(0, 35);

                ingredients[i] = new Ingredient(title, unit, Title);
            }

            Ingredients = ingredients;

            var stepRecipeBody = recipeBody.QuerySelector("div[itemprop='recipeInstructions'][itemtype='http://schema.org/ItemList']");
            if (stepRecipeBody != null)
            {
                var stepCollection = stepRecipeBody.QuerySelectorAll("div");
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
                StepsRecipe = stepRecipesBoxes.ToArray();
            }
           

            var rcpAuthorTimeBody = recipeBody
                .QuerySelector("div.rcpAuthorTime");

            string authorName = rcpAuthorTimeBody.QuerySelector("span[id='autorName']").TextContent;

            double prepMinutes = ConvertToMinutes(rcpAuthorTimeBody.QuerySelector("meta[itemprop='cookTime']").Attributes[1].Value);

            string[] portions = recipeBody
                .QuerySelector("em[itemprop='recipeYield']")
                .TextContent
                .Remove(0, 19)
                .Split('-') ?? null;

            int countPortions = 0;

            if (portions != null)
                countPortions = int.Parse(RemoveSymbols(portions[0]));

            // На повар.ру нет информации(
            CPFC CPFC = null;

            Additional = new Additional(authorName, countPortions, prepMinutes, CPFC);


            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients,
                StepsRecipe,
                Additional);
        }

        private string RemoveSymbols(string line)
        {
            string newLine = string.Empty;

            foreach (var item in line)
            {
                if (char.IsDigit(item))
                    newLine += item;
            }

            return newLine;
        }

        /// <see cref="IParserRecipe{T}.ConvertToMinutes(string)"/>
        public double ConvertToMinutes(string inputLine)
        {
            // Format: PT<min>M
            return double.Parse(inputLine[2..^1]);
        }

    }
}