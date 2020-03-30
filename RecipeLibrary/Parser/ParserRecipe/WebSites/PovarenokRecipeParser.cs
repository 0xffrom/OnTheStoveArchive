using System;
using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarenokRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Picture TitlePicture { get; set; }
        private string Description { get; set; }
        private IngredientBox[] IngredientsBoxes { get; set; }
        private StepRecipeBox[] StepRecipesBoxes { get; set; }
        private AdditionalBox Additional { get; set; }

        private const string WhiteSpaceBug = "  ";

        public RecipeFull Parse(IHtmlDocument document)
        {
            var recipeBody = document
                .QuerySelectorAll("article")
                .FirstOrDefault(element => element.ClassName != null && element.ClassName == "item-bl item-about");

            // recipeBody =>  главный фрейм с рецептом, если он существует - работаем с ним, иначе - рецепта не сущесвует.
            if (recipeBody == null)
                return new RecipeFull(string.Empty, Title, TitlePicture, Description, IngredientsBoxes,
                    StepRecipesBoxes,
                    Additional);


            #region Title

            Title = recipeBody.QuerySelector("h1").TextContent;

            TitlePicture = new Picture(recipeBody.QuerySelectorAll("div")
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

            var ingredientBoxes = new IngredientBox[countIngredientTitles];

            for (int i = 0; i < countIngredientTitles; i++)
            {
                // "Время приготовления" и "Количество порций".
                int count = 0;

                var p = ingredientBody.QuerySelectorAll("p")
                    .Select(item => item.TextContent).ToArray();

                if (p.Any(element => element.Contains("Время приготовления:")))
                    count++;
                if (p.Any(element => element.Contains("Количество порций:")))
                    count++;


                string titleIngredient;

                var titleBody = ingredientBody.QuerySelectorAll("p").ToArray();
                if (titleBody.Length - count == 0)
                    titleIngredient = Title;

                else
                    titleIngredient = ingredientBody.QuerySelectorAll("p")
                        .Select(item => item.TextContent).ToArray()[i];


                var ingredientsArray = ingredientBody.QuerySelectorAll("ul")
                    .ToArray()[i]
                    .QuerySelectorAll("li")
                    .ToArray();

                Ingredient[] ingredients = new Ingredient[ingredientsArray.Length];

                for (int j = 0; j < ingredientsArray.Length; j++)
                {
                    string name = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                            item.Attributes[0] != null && item.Attributes[0].Value == ("name"))
                        .Select(item => item.TextContent).First();

                    string unit = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                            item.Attributes[0] != null && item.Attributes[0].Value == ("amount"))
                        .Select(item => item.TextContent).First();

                    name += ingredientsArray[j].TextContent.Replace(name, string.Empty)
                        .Replace(unit, string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace(WhiteSpaceBug, string.Empty)
                        .Replace("—", String.Empty);

                    Ingredient ingredient = new Ingredient(name, unit);
                    ingredients[j] = ingredient;
                }

                IngredientBox ingredientBox = new IngredientBox(titleIngredient, ingredients);

                ingredientBoxes[i] = ingredientBox;

                IngredientsBoxes = ingredientBoxes;
            }

            #endregion

            #region StepRecipeBox

            var recipesArray = recipeBody.QuerySelectorAll("div")
                .Where(item => item.ClassName != null && item.ClassName == ("cooking-bl"))
                .ToArray();

            int countRecipes = recipesArray.Length;

            StepRecipeBox[] stepRecipeBoxes = new StepRecipeBox[countRecipes];

            for (int i = 0; i < countRecipes; i++)
            {
                string pictureUrl = recipesArray[i]?.FirstElementChild?.FirstElementChild?.Attributes[2]?.Value;
                string description = recipesArray[i]?.LastElementChild?.FirstElementChild?.TextContent;

                var pictureBox = new PictureBox(new Picture[1] {new Picture(pictureUrl)});

                var stepRecipeBox = new StepRecipeBox(description, pictureBox);

                stepRecipeBoxes[i] = stepRecipeBox;
            }

            StepRecipesBoxes = stepRecipeBoxes;

            #endregion

            #region AdditionalBox

            var additionalBody = recipeBody.QuerySelectorAll("div")
                .LastOrDefault(element =>
                    element.NextElementSibling?.ClassName != null &&
                    element.NextElementSibling.ClassName == ("article-tags"));

            if (additionalBody == null)
                return new RecipeFull(string.Empty, Title, TitlePicture, Description, IngredientsBoxes,
                    StepRecipesBoxes,
                    Additional);


            var imagesArray = additionalBody.QuerySelectorAll("a")
                .ToArray();

            Picture[] pictures = new Picture[imagesArray.Length];

            for (int i = 0; i < imagesArray.Length; i++)
            {
                Picture picture = new Picture("http://www.povarenok.ru" + imagesArray[i].Attributes[0].Value);

                pictures[i] = picture;
            }

            PictureBox picturesBox = new PictureBox(pictures);

            string textAdditional = additionalBody.TextContent
                .Replace(WhiteSpaceBug, String.Empty);

            Video video = new Video(recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == ("video-bl"))
                .Select(element => element.FirstElementChild.FirstElementChild.Attributes[2].Value)
                .FirstOrDefault());

            Additional = new AdditionalBox(textAdditional, picturesBox, video);

            #endregion


            return new RecipeFull(string.Empty, Title, TitlePicture, Description, IngredientsBoxes,
                StepRecipesBoxes,
                Additional);
            ;
        }
    }
}