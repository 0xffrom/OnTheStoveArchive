using System;
using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimdomaRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Picture TitlePicture { get; set; }
        private string Description { get; set; }
        private IngredientBox[] IngredientsBoxes { get; set; }
        private StepRecipeBox[] StepRecipesBoxes { get; set; }
        private AdditionalBox AdditionalBox { get; set; }
        public RecipeFull Parse(IHtmlDocument document)
        {
          
            
            
            

            return new RecipeFull(string.Empty, Title, TitlePicture, Description, IngredientsBoxes,
                StepRecipesBoxes,
                AdditionalBox);
        }
    }
}