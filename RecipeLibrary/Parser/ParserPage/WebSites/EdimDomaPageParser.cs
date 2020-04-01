using System;
using System.Linq;
using System.Reflection;
using AngleSharp.Html.Dom;
using ObjectsLibrary.Objects;
using ObjectsLibrary.Objects.Boxes.Elements;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    class EdimDomaPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document, IParserPageSettings settings)
        {
            try
            {
                var recipeCards = document.QuerySelectorAll("article")
                    .Where(x => x.ClassName != null && x.ClassName == "card").Select(x => x.FirstElementChild)
                    .ToArray();

                double indexStartPopularity = settings.IndexPopularity;

                return (from recipeCard in recipeCards
                    let url = "https://www.edimdoma.ru/" + recipeCard.Attributes[0].Value
                    let title = recipeCard.FirstElementChild.FirstElementChild.Attributes[1].Value
                    let pictureUrl = recipeCard.FirstElementChild.FirstElementChild.Attributes[2].Value
                    let picture = new Picture(pictureUrl)
                    let indexPopularity = indexStartPopularity -= settings.IndexStep
                    select new RecipeShort(title, picture, url, indexStartPopularity)).ToArray();
            }
            catch (Exception exp)
            {
                throw  new ParserException(exp.Message, this.ToString());
            }
        }
    }
}