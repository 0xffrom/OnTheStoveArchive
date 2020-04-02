using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using ObjectsLibrary.Objects;
using ObjectsLibrary.Objects.Boxes;
using ObjectsLibrary.Objects.Boxes.Elements;
using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Picture TitlePicture { get; set; }
        private string Description { get; set; }
        private Ingredient[] Ingredients { get; set; }
        private StepRecipeBox[] StepRecipesBoxes { get; set; }
        private AdditionalBox AdditionalBox { get; set; }
        
        private const string WhiteSpaceBug = "                                 - ";
        public RecipeFull Parse(IHtmlDocument document)
        {
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

            TitlePicture = new Picture(recipeBody
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
                    .Remove(0,35);
                
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
                .Where(x=> x.ClassName != null)
                .ToArray();
            
            List<StepRecipeBox> stepRecipesBoxes = new List<StepRecipeBox>(stepCollection.Length / 3);

            foreach (var step in stepCollection)
            {
                switch (step.ClassName)
                {
                    case "detailed_step_photo_big":
                    {
                        var firstEl = step.FirstElementChild;
                        
                        string description = firstEl.Attributes[0].Value;

                        string pictureUrl = firstEl.Attributes[3].Value;

                        stepRecipesBoxes.Add(new StepRecipeBox(description,
                            new PictureBox(new Picture[1] {new Picture(pictureUrl)})));
                        break;
                    }
                    case "detailed_step_description_big noPhotoStep":
                    {
                        string description = step.TextContent;
                        stepRecipesBoxes.Add(new StepRecipeBox(description,
                            new PictureBox(new Picture[1] {new Picture("none picture")})));
                        break;
                    }
                }
            }

            StepRecipesBoxes = stepRecipesBoxes.ToArray();
            
            #endregion

            #region Additional

            // На сайте это называется "Совет от повара", там может быть только какой-то мини совет
            // Видео или фото не предполгаются.

            var descriptionAdditional = document.QuerySelectorAll("span")
                .Where(x => x.ClassName != null && x.ClassName == "detailed_full")
                .Select(x => x.TextContent).FirstOrDefault();

            var videoAdditional = document
                .QuerySelectorAll("iframe").FirstOrDefault(x => x.ClassName != null && x.ClassName == "youtubeVideo");
            
            var videoUrl = videoAdditional?.Attributes[2].Value;
            
            AdditionalBox = new AdditionalBox(descriptionAdditional, new PictureBox(null), new Video(videoUrl));

            #endregion

            return new RecipeFull(string.Empty, Title, TitlePicture, Description, Ingredients,
                StepRecipesBoxes,
                AdditionalBox);
        }
    }
}