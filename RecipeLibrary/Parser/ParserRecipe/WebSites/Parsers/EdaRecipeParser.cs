using AngleSharp.Html.Dom;
using Newtonsoft.Json.Linq;
using ObjectsLibrary.Components;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdaRecipeParser : IParserRecipe<RecipeFull>
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

        /// <see cref="RecipeFull.StepsRecipe"/>
        private StepRecipe[] StepsRecipe { get; set; }

        /// <see cref="RecipeFull.Additional"/>
        private Additional Additional { get; set; }

        public RecipeFull Parse(IHtmlDocument document, IParserRecipeSettings parserRecipeSettings)
        {
            Url = parserRecipeSettings.Url;
            Title = document.QuerySelector("h1.recipe__name.g-h1").TextContent.Trim().Replace("\n", "");

            var divsDescription = document.QuerySelectorAll("p.recipe__description");
            var divsHistory = document.QuerySelectorAll("div.recipe__story");

            foreach (var description in divsDescription)
            {
                Description += description.TextContent.Trim().Replace("\n","") + Environment.NewLine;
            }

            foreach (var history in divsHistory)
            {
                Description += history.TextContent.Trim().Replace("\n", "").Replace("Читать полностью","") + Environment.NewLine;
            }

            var pIngredients = document.QuerySelectorAll("div.ingredients-list.layout__content-col > div.ingredients-list__content > p.ingredients-list__content-item.content-item.js-cart-ingredients");

            List<Ingredient> ingredients = new List<Ingredient>(pIngredients.Length);

            foreach (var pIngredient in pIngredients)
            {
                JObject jObject = JObject.Parse(pIngredient.Attributes[1].Value);

                string title = jObject["name"].Value<string>();
                string unit = jObject["amount"].Value<string>();

                ingredients.Add(new Ingredient(title, unit, Title));
            }

            Ingredients = ingredients.ToArray();

            var liSteps = document.QuerySelectorAll("ul.recipe__steps > li.instruction.clearfix.js-steps__parent");

            List<StepRecipe> stepRecipes = new List<StepRecipe>(liSteps.Length);

            foreach (var liStep in liSteps)
            {
                string imageUrl =  liStep.QuerySelector("div.lazy-load-container")?.Attributes[3]?.Value ?? "";
                string description = liStep.QuerySelector("span.instruction__description.js-steps__description").TextContent.Trim();
                stepRecipes.Add(new StepRecipe(description, new Image(imageUrl)));
            }

            StepsRecipe = stepRecipes.ToArray();

            // Берём последнюю итоговую фотографию рецепта:
            var imageBody = document.QuerySelector("div.g-print-visible").QuerySelector("img");

            if (imageBody?.Attributes[0] != null)
                TitleImage = new Image(imageBody.Attributes[0].Value);
            else
                TitleImage = stepRecipes[stepRecipes.Count-1]?.Image ??
                    new Image("https://s2.eda.ru/StaticContent/All/w/29261930/assets/images/png/404-ingr.png");

            double prepMinutes = ConvertToMinutes(document.QuerySelectorAll("span.info-pad__item")
                .FirstOrDefault(x=> x.FirstElementChild?.ClassName == "timer").QuerySelector("span.info-text")?.TextContent ?? "0");
            int countPortions = int.Parse(document.QuerySelectorAll("span.info-pad__item")
                .FirstOrDefault(x => x.FirstElementChild?.ClassName == "portion").QuerySelector("span.info-text.js-portions-count-print")?.TextContent.Split(' ')[0] ?? "0");
            string authorName = document.QuerySelector("p.author-name > span")?.TextContent ?? string.Empty;

            var cpfcList = document.QuerySelectorAll("li > p.nutrition__weight");

            double calories = double.Parse(cpfcList[0]?.TextContent?.Replace('.', ',') ?? "0");
            double protein = double.Parse(cpfcList[1]?.TextContent?.Replace('.', ',') ?? "0");
            double fats = double.Parse(cpfcList[2]?.TextContent?.Replace('.', ',') ?? "0");
            double carbohydrates = double.Parse(cpfcList[3]?.TextContent?.Replace('.', ',') ?? "0");

            var videoScr = document.QuerySelector("iframe[frameborder='0']");
            string videoUrl = string.Empty;
            if (videoScr != null)
                videoUrl = videoScr.Attributes[3]?.Value ?? "";

            Additional = new Additional(authorName, countPortions, prepMinutes, new CPFC(calories, protein, fats, carbohydrates), videoUrl);

            return new RecipeFull(Url, Title, TitleImage, Description, Ingredients, StepsRecipe, Additional);
        }

        public double ConvertToMinutes(string inputLine)
        {
            if (inputLine is null)
                return 0;

            inputLine = inputLine.Replace(" и", String.Empty);

            string[] arrayWords = inputLine.Split(' ');

            double minutes = 0;
            for (int i = 1; i < arrayWords.Length; i += 2)
            {
                if (arrayWords[i].Contains('м'))
                    minutes += int.Parse(arrayWords[i - 1]);
                else if (arrayWords[i].Contains('ч'))
                    minutes += int.Parse(arrayWords[i - 1]) * 60;
                else if (arrayWords[i].Contains('д'))
                    // 60 * 24 = 1440‬
                    minutes += int.Parse(arrayWords[i - 1]) * 1440;
            }

            return minutes;
        }

    }
}