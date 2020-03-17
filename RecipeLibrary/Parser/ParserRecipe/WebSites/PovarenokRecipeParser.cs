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
        private string Url { get; set; }
        private string Title { get; set; }
        private Picture TitlePicture { get; set; }
        private string Description { get; set; }
        private IngredientBox[] IngredientsBoxes { get; set; }
        private StepRecipeBox[] StepRecipesBoxes { get; set; }
        private AdditionalBox Additional { get; set; }

        public RecipeFull Parse(IHtmlDocument document)
        {
            #region Url

            // TODO: дописать поиск URL.
            
            #endregion

            var recipeBody = document.QuerySelectorAll("article")
                .Where(element => element.ClassName != null && element.ClassName == "item-bl item-about")
                .ToArray()[0];

            #region Title

            string title = recipeBody.QuerySelector("h1").TextContent;
            string urlTitlePicture = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "m-img")
                .Select(element => element.FirstElementChild.Attributes[1].Value)
                .ToArray()[0];

            Picture titlePicture = new Picture(urlTitlePicture);

            Title = title;
            TitlePicture = titlePicture;

            #endregion

            #region Description

            Description = document.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "article-text")
                .Select(element => element.TextContent).ToArray()[0]
                .Replace("\n", String.Empty)
                .Replace("  ", String.Empty);

            #endregion

            #region IngredientBox

            var ingredientBody = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "ingredients-bl")
                .ToArray()[0];

            int countIngredientTitles = ingredientBody.QuerySelectorAll("ul").ToArray().Length;

            IngredientBox[] ingredientBoxes = new IngredientBox[countIngredientTitles];

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
                    //string jumpSpaceBug = ("\n");
                    //string whiteSpaceBug = ("  ");

                    //string name = ingredientsArray[j].FirstElementChild.FirstElementChild.TextContent
                    //    .Replace(jumpSpaceBug, String.Empty)
                    //    .Replace(whiteSpaceBug, String.Empty);

                    //string unit = ingredientsArray[j].TextContent
                    //    .Replace(name, String.Empty)
                    //    .Replace(whiteSpaceBug, String.Empty)
                    //    .Replace(jumpSpaceBug, String.Empty)
                    //    .Replace("—", " ")
                    //   .Replace("/", String.Empty);

                    string name = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                            item.Attributes[0] != null && item.Attributes[0].Value.Contains("name"))
                        .Select(item => item.TextContent).First();

                    string unit = ingredientsArray[j].QuerySelectorAll("span").Where(item =>
                            item.Attributes[0] != null && item.Attributes[0].Value.Contains("amount"))
                        .Select(item => item.TextContent).First();

                    name += ingredientsArray[j].TextContent.Replace(name, string.Empty)
                        .Replace(unit, String.Empty)
                        .Replace("\n", String.Empty)
                        .Replace("  ", String.Empty)
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
                .Where(item => item.ClassName != null && item.ClassName.Contains("cooking-bl"))
                .ToArray();

            int countRecipes = recipesArray.Length;
            StepRecipeBox[] stepRecipeBoxes = new StepRecipeBox[countRecipes];

            for (int i = 0; i < countRecipes; i++)
            {
                string pictureUrl = recipesArray[i].FirstElementChild.FirstElementChild.Attributes[2].Value;
                string description = recipesArray[i].LastElementChild.FirstElementChild.TextContent;

                Picture picture = new Picture(pictureUrl);
                PictureBox pictureBox = new PictureBox(new Picture[1] {picture});

                StepRecipeBox stepRecipeBox = new StepRecipeBox(description, pictureBox);

                stepRecipeBoxes[i] = stepRecipeBox;
            }

            StepRecipesBoxes = stepRecipeBoxes;

            #endregion

            #region AdditionalBox

            var additionalBody = recipeBody.QuerySelectorAll("div")
                .LastOrDefault(element =>
                    element.NextElementSibling?.ClassName != null &&
                    element.NextElementSibling.ClassName.Contains("article-tags"));

            if (additionalBody != null)
            {
                var imagesArray = additionalBody.QuerySelectorAll("a")
                    .ToArray();

                Picture[] pictures = new Picture[imagesArray.Length];

                for (int i = 0; i < imagesArray.Length; i++)
                {
                    Picture picture = new Picture("http://www.povarenok.ru" + imagesArray[i].Attributes[0].Value);

                    pictures[i] = picture;
                }

                PictureBox picturesBox = new PictureBox(pictures);

                string whiteSpaceBug = ("  ");


                string textAdditional = additionalBody.TextContent
                    .Replace(whiteSpaceBug, String.Empty);

                var videoArray = recipeBody.QuerySelectorAll("div")
                    .Where(element => element.ClassName != null && element.ClassName.Contains("video-bl"))
                    .Select(element => element.FirstElementChild.FirstElementChild.Attributes[2].Value)
                    .ToArray();

                string videoUrl = null;
                if (videoArray.Length > 0)
                    videoUrl = videoArray[0];

                Video video = new Video(videoUrl);
                AdditionalBox additionalBox = new AdditionalBox(textAdditional, picturesBox, video);

                Additional = additionalBox;
            }

            #endregion


            RecipeFull recipeFull = new RecipeFull(Url, Title, TitlePicture, Description, IngredientsBoxes,
                StepRecipesBoxes,
                Additional);

            return recipeFull;
        }
        
    }
}