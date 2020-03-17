using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    
    public class PovarRecipeParser
    {
        private string _url;
        private string _title;
        private Picture _titlePicture;
        private string _description;
        private IngredientBox[] _ingredientBoxes;
        private StepRecipeBox[] _stepRecipeBoxes;
        private AdditionalBox _additionalBox;

        public PovarRecipeParser(IHtmlDocument document)
        {
            
            // TODO: Проверить на ошибки все методы.
            
            // The piece of html code our recipe.
            var recipeBody = document
                .QuerySelectorAll("div")
                .First(el => el.ClassName != null && el.ClassName == "cont_area");
            
            #region Url

            // TODO: append to find url;


            #endregion

            #region Title

             _title = recipeBody.QuerySelectorAll("h1")
                .Where(el => el.ClassName != null
                             && el.ClassName == "detailed"
                             && el.Attributes[1] != null
                             && el.Attributes[1].Value == "name")
                .Select(el => el.TextContent)
                .First();
             
             #endregion

             #region TitlePicture

             _titlePicture = new Picture(recipeBody
                 .QuerySelectorAll("div")
                 .First(el => el.ClassName != null && el.ClassName == "bigImgBox")
                 .QuerySelector("a")
                 .FirstElementChild.Attributes[0].Value);

             #endregion
        }
    }
}