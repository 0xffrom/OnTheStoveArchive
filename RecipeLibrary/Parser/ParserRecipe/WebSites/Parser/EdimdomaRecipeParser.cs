using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using System;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimdomaRecipeParser : IParserRecipe<RecipeFull>
    {
        /// <see cref="RecipeFull.Url"/>
        private string Url { get; set; }

        /// <see cref="RecipeFull.Title"/>
        private string Title { get; set; }

        /// <see cref="RecipeFull.TitleImage"/>
        private Image TitleImage { get; set; }

        /// <see cref="RecipeFull.Description"/>
        private string Description { get; set; }

        /// <see cref="RecipeFull.Ingredients"/>
        private Ingredient[] Ingredients { get; set; }

        /// <see cref="RecipeFull.StepRecipesBoxes"/>
        private StepRecipe[] StepRecipesBoxes { get; set; }

        /// <see cref="RecipeFull.Additional"/>
        private Additional Additional { get; set; }

        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            Url = parserRecipeSettings.Url;






            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients,
                StepRecipesBoxes,
                Additional);
        }

        public double ConvertToMinutes(string inputLine)
        {
            throw new NotImplementedException();
        }

    }
}