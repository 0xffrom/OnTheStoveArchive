using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{

    public class PovarRecipeParser
    {
        private string _title;
        private Picture _titlePicture;
        private string _description;
        private IngredientBox[] _ingredientBoxes;
        private StepRecipeBox[] _stepRecipeBoxes;
        private AdditionalBox _additionalBox;

        public PovarRecipeParser(IHtmlDocument document)
        {
            var recipeBody = document
                .QuerySelectorAll("div")
                .First(el => el.ClassName != null && el.ClassName == "cont_area");


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

            #region Description
            _description = recipeBody
                .QuerySelectorAll("span")
                .Where(el => el.ClassName != null && el.ClassName == "detailed_full")
                .First()
                .QuerySelector("span").TextContent;
            #endregion

            #region Ingredients

            var ingredientsBody = recipeBody
                .QuerySelectorAll("ul")
                .Where(x => x.ClassName != null && x.ClassName == "detailed_ingredients")
                .ToArray();

            Ingredient[] ingredients = new Ingredient[ingredientsBody.Length];

            for (int i = 0; i < ingredients.Length; i++)
                ingredients[i] = new Ingredient(ingredientsBody[i].Attributes[1].Value, ingredientsBody[i]
                    .TextContent
                    .Replace(ingredientsBody[i].Attributes[1].Value, string.Empty)
                    .Replace("                                 — ", string.Empty));
            

            _ingredientBoxes = new IngredientBox[1] { new IngredientBox(_title, ingredients) };
            #endregion

            #region StepRecipe
            //var stepRecipeBody = 
            #endregion
        }
    }
}