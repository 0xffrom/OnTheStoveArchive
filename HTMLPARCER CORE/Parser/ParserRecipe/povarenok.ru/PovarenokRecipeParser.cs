using System;
using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.ParseRecipe
{
    public class PovarenokRecipeParser : IParserRecipe<RecipeFull>
    {
        //public string Title { get; }
        //public Picture TitlePicture { get; }
        //public IngredientBox[] IngredientsBoxes { get; }
        //public StepRecipeBox[] StepRecipesBoxes { get; }
        //public AdditionalBox Additional { get; }
        
        public RecipeFull Parse(IHtmlDocument document)
        {
            var recipeBody = document.QuerySelectorAll("article")
                .Where(element => element.ClassName != null && element.ClassName == "item-bl item-about")
                .ToArray()[0];

            string title = recipeBody.QuerySelector("h1").TextContent;
            string urlTitlePicture = recipeBody.QuerySelectorAll("div")
                .Where(element => element.ClassName != null && element.ClassName == "m-img")
                .Select(element => element.FirstElementChild.Attributes[1].Value)
                .ToArray()[0];
            
            Picture titlePicture = new Picture(urlTitlePicture);

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
            
            // TODO: Подумать насчёт энергетической ценности.
            var recipesArray
            StepRecipeBox[] stepRecipeBoxes = new StepRecipeBox[];







            throw new System.NotImplementedException();
        }
    }
}