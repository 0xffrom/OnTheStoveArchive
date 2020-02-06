using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
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

            int countIngredientTitles = ingredientBody.QuerySelectorAll("p").
                // TODO: Не закончено.
            






            throw new System.NotImplementedException();
        }
    }
}