using System;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarenokParserPage : IParser<RecipeFull[]>
    {
        private string webSite = "povarenok.ru";
        private string title;
        private string titlePicture;
        private string url;
        private Ingredient[] ingredients;
        private StepRecipe[] stepsOfRecipe;
        private string introductionContent;
        private string endContent;
        
        public RecipeFull[] Parse(IHtmlDocument document)
        {
            Console.WriteLine("[DEBUG]: code 1");
            var titleArray = document.QuerySelectorAll("h1").Where(item =>
                item.ParentElement.ParentElement.ClassList.Contains(
                    "item-about")).ToArray();

            Console.WriteLine($"[DEBUG]: {titleArray.Length}");

            var titlePictureArray = document.QuerySelectorAll("img").Where(item =>
                item.ParentElement.ClassList.Contains(
                    "m-img")).ToArray();

            Console.WriteLine($"[DEBUG]: {titlePictureArray.Length}");

            var introductionContentArray = document.QuerySelectorAll("p").Where(item =>
                item.ParentElement.ClassList.Contains(
                    "article-text")).ToArray();
            Console.WriteLine($"[DEBUG]: {introductionContentArray.Length}");


            var ingridientsArray = document.QuerySelectorAll("div").Where(item =>
                item.ClassList != null &&
                item.ClassList.Contains("ingredients-bl")).ToArray();

            Console.WriteLine($"[DEBUG]: {ingridientsArray[0].TextContent.Replace("\n", "").Replace("  ", "")}");


            var stepsOfRecipeArray = document.QuerySelectorAll("div")
                .Where(item => item.ClassList != null && item.ClassList.Contains("cooking-bl")).ToArray();

            Console.WriteLine($"[DEBUG]: {stepsOfRecipeArray.Length}");


            var endContentArray = document.QuerySelectorAll("div").Where(item =>
                    item.ParentElement.ParentElement.ClassName != null &&
                    item.ParentElement.ParentElement.ClassName.Contains("item-bl item-about"))
                .ToArray();
            
            Console.WriteLine($"[DEBUG]: {endContentArray.Length}");
            
            title = titleArray[0].TextContent;
            titlePicture = titlePictureArray[0].Attributes[1].Value;
            introductionContent = introductionContentArray[0].TextContent;
            Console.WriteLine(
                $"[DEBUG]: url: {url}, title: {title}, titlePicture: {titlePicture}, introductionContent: {introductionContent}");
            ingredients = new Ingredient[ingridientsArray.Length];

            for (int i = 0; i < ingredients.Length; i++)
            {
                string name = ingridientsArray[i].FirstElementChild.FirstElementChild.TextContent.Replace("\n", "").Replace("  ", "");
                string unit = ingridientsArray[i].FirstElementChild.LastElementChild.TextContent.Replace("\n", "").Replace("  ", "");
                if (unit == name)
                    unit = "По вкусу.";
                Console.WriteLine($"[DEBUG]: id:{i} name:{name}, unit: {unit}");
                ingredients[i] = new Ingredient(title, name, unit);
            }

            stepsOfRecipe = new StepRecipe[stepsOfRecipeArray.Length];
            for (int i = 0; i < stepsOfRecipe.Length; i++)
            {
                string description = stepsOfRecipeArray[i].LastElementChild.TextContent;
                string urlPicture = stepsOfRecipeArray[i].FirstElementChild.Attributes[2].Value;
                Console.WriteLine($"[DEBUG]: id:{i} name:{description}, unit{stepsOfRecipeArray}");
                stepsOfRecipe[i] = new StepRecipe(description, urlPicture);
            }


            return new RecipeFull[]
            {
                new RecipeFull(webSite, title, titlePicture, url,
                    ingredients, stepsOfRecipe, introductionContent, endContent)
            };
        }

        public int GetCount() => 1;
    }
}