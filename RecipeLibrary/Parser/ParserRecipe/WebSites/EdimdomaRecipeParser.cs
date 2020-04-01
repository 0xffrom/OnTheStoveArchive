using System;
using System.Linq;
using AngleSharp.Html.Dom;
using ObjectsLibrary.Objects;
using ObjectsLibrary.Objects.Boxes;
using ObjectsLibrary.Objects.Boxes.Elements;
using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimdomaRecipeParser : IParserRecipe<RecipeFull>
    {
        private string Title { get; set; }
        private Picture TitlePicture { get; set; }
        private string Description { get; set; }
        private Ingredient[] Ingredients{ get; set; }
        private StepRecipeBox[] StepRecipesBoxes { get; set; }
        private AdditionalBox AdditionalBox { get; set; }
        public RecipeFull Parse(IHtmlDocument document)
        {
          
            
            
            

            return new RecipeFull(string.Empty, Title, TitlePicture, Description, Ingredients,
                StepRecipesBoxes,
                AdditionalBox);
        }
    }
}