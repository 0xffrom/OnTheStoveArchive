using System;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.ParseRecipe
{
    public class PovarenokRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Picture TitlePicture { get; set;}
        private IngredientBox[] IngredientsBoxes { get; set;}
        private StepRecipeBox[] StepRecipesBoxes { get; set;}
        private AdditionalBox Additional { get; set;}
        
        public RecipeFull Parse(IHtmlDocument document)
        {
            #region Title

            var recipeBody = document.QuerySelectorAll("article")
                .Where(element => element.ClassName != null && element.ClassName == "item-bl item-about")
                .ToArray()[0];

            string title = recipeBody.QuerySelector("h1").TextContent;
            string urlTitlePicture = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "m-img")
                .Select(element => element.FirstElementChild.Attributes[1].Value)
                .ToArray()[0];
            
            Picture titlePicture = new Picture(urlTitlePicture);


            #endregion

            #region IngredientBox

            var ingredientBody = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "ingredients-bl")
                .ToArray()[0];

            int countIngredientTitles = ingredientBody.QuerySelectorAll("ul").ToArray().Length;
            
            IngredientBox[] ingredientBoxes = new IngredientBox[countIngredientTitles];
            
            for (int i = 0; i < countIngredientTitles; i++)
            {
                string titleIngredient = ingredientBody.QuerySelectorAll("p")
                    .Select(item => item.TextContent).ToArray()[i];

                var ingredientsArray = ingredientBody.QuerySelectorAll("ul")
                    .ToArray()[i]
                    .QuerySelectorAll("li")
                    .Select(item => item.FirstElementChild)
                    .ToArray();
                
                Ingredient[] ingredients = new Ingredient[ingredientsArray.Length];
                
                for (int j = 0; j < ingredientsArray.Length; j++)
                {
                    string whiteSpaceBug = ("\n                ");
                    
                    string name = ingredientsArray[j].FirstElementChild.FirstElementChild.TextContent
                        .Replace(whiteSpaceBug, String.Empty);
                    
                    string unit = ingredientsArray[j].TextContent
                        .Replace(name, string.Empty)
                        .Replace(whiteSpaceBug, String.Empty);
                    
                    Ingredient ingredient = new Ingredient(name, unit);

                    ingredients[j] = ingredient;
                }
                
                IngredientBox ingredientBox = new IngredientBox(titleIngredient, ingredients);
                
                ingredientBoxes[i] = ingredientBox;
            }

            #endregion

            #region StepRecipeBox

            var recipesArray = ingredientBody.QuerySelectorAll("div")
                .Where(item =>
                    item.ClassName != null && item.ClassName.Contains("cooking-bl") &&
                    item.Attributes[1].Value != null && item.Attributes[1].Value.Contains("recipeInstructions")).ToArray();
            int countRecipes = recipesArray.Length;
            
            StepRecipeBox[] stepRecipeBoxes = new StepRecipeBox[countRecipes];

            for (int i = 0; i < countRecipes; i++)
            {
                string pictureUrl = recipesArray[i].FirstElementChild.FirstElementChild.Attributes[2].Value;
                string description = recipesArray[i].LastElementChild.TextContent;
                
                Picture picture = new Picture(pictureUrl);
                PictureBox pictureBox = new PictureBox(new Picture[1] {picture});
                
                StepRecipeBox stepRecipeBox = new StepRecipeBox(description, pictureBox);

                stepRecipeBoxes[i] = stepRecipeBox;
            }

            #endregion

            #region AdditionalBox

            var additionalBody = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName == null && element.Attributes.Length == 0).ToArray()[0];

            var imagesArray = additionalBody.QuerySelectorAll("img")
                .Select(item => "http://www.povarenok.ru/" + item.Attributes[2].Value)
                .ToArray();
            
            Picture[] pictures = new Picture[imagesArray.Length];
            
            for (int i = 0; i < imagesArray.Length; i++)
            {
                Picture picture = new Picture(imagesArray[i]);

                pictures[i] = picture;
            }
            
            PictureBox picturesBox = new PictureBox(pictures);

            string textAdditional = additionalBody.TextContent;

            string videoUrl = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName.Contains("video-bl"))
                .Select(element => element.FirstElementChild.FirstElementChild.Attributes[0].Value)
                .ToArray()[0];
            
            
            Video video = new Video(videoUrl);
            AdditionalBox additionalBox = new AdditionalBox(textAdditional, picturesBox, video);
            
            Additional = additionalBox;
            #endregion
            
            // TODO: Допилить ретурн





            throw new System.NotImplementedException();
        }
        
        // TODO: Подумать насчёт энергетической ценности.
    }
}