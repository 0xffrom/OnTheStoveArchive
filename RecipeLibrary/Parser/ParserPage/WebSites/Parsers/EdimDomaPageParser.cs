using AngleSharp.Html.Dom;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserPage.Core;
using System.Linq;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    class EdimDomaPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            var recipeCards = document.QuerySelectorAll("article")
                    .Where(x => x.ClassName != null && x.ClassName == "card")
                    .Select(x => x.FirstElementChild);

            double indexStartPopularity = settings.IndexPopularity;

            return (from recipeCard in recipeCards
                    let url = "https://www.edimdoma.ru/" + recipeCard.Attributes[0].Value
                    let title = recipeCard.FirstElementChild.FirstElementChild.Attributes[1].Value
                    let pictureUrl = recipeCard.FirstElementChild.FirstElementChild.Attributes[2].Value
                    let image = new Image(pictureUrl)
                    let indexPopularity = indexStartPopularity -= settings.IndexStep
                    select new RecipeShort(title, image, url, indexStartPopularity)).ToArray();
        }
    }
}