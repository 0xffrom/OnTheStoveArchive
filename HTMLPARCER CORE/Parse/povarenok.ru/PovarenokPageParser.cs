using System;
using System.Collections.Generic;
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
        private Ingredient[] ingredients;
        private StepRecipe[] stepsOfRecipe;
        private string introductionContent;
        private string endContent;

        public RecipeFull[] Parse(IHtmlDocument document)
        {

            var titleArray = document.QuerySelectorAll("h1").Where(item =>
                item.ParentElement.ParentElement.ClassList.Contains(
                    "item-about")).ToArray();

            var titlePictureArray = document.QuerySelectorAll("img").Where(item =>
                item.ParentElement.ClassList.Contains(
                    "m-img")).ToArray();

            var introductionContentArray = document.QuerySelectorAll("p").Where(item =>
                item.ParentElement.ClassList.Contains(
                    "article-text")).ToArray();

            var ingridientsArray = document.QuerySelectorAll("div").Where(item =>
                item.ClassList != null &&
                item.ClassList.Contains("ingredients-bl")).ToArray();
            

            var stepsOfRecipeArray = document.QuerySelectorAll("div")
                .Where(item => item.ClassList != null && item.ClassList.Contains("cooking-bl")).ToArray();


            var endContentArray = document.QuerySelectorAll("div").Where(item =>
                    item.ParentElement.ParentElement.ClassName != null &&
                    item.ParentElement.ParentElement.ClassName.Contains("item-bl item-about"))
                .ToArray();


            title = titleArray[0].TextContent;
            titlePicture = titlePictureArray[0].Attributes[1].Value;
            introductionContent = introductionContentArray[0].TextContent;

            var pIngridientsArray = ingridientsArray[0].QuerySelectorAll("p").ToArray();
            var ulIngridientsArray = ingridientsArray[0].QuerySelectorAll("ul").ToArray();
            List<Ingredient> ingridientsList = new List<Ingredient>();

            for (int i = 0; i < ulIngridientsArray.Length; i++)
            {
                var liIngridientsArray = ulIngridientsArray[i].QuerySelectorAll("li").ToArray();
                foreach (var li in liIngridientsArray)
                {
                    string text = li.TextContent.Replace("\n", "").Replace("  ", "");
                    string name;
                    string unit;
                    if (text.Contains("—"))
                    {
                        int index = text.IndexOf('—');
                        name = text.Substring(0, index);
                        unit = text.Substring(index + 1);
                    }
                    else
                    {
                        name = text;
                        unit = null;
                    }

                    ingridientsList.Add(new Ingredient(pIngridientsArray[i].TextContent, name, unit));
                }

            }

            ingredients = ingridientsList.ToArray();



            stepsOfRecipe = new StepRecipe[stepsOfRecipeArray.Length];
            for (int i = 0; i < stepsOfRecipe.Length; i++)
            {
                string description = stepsOfRecipeArray[i].LastElementChild.TextContent.Replace("  ", "").Replace("\n", "");
                string urlPicture = stepsOfRecipeArray[i].QuerySelector("img").Attributes[2].Value;
                Console.WriteLine($"[DEBUG]: id:{i} name:{description}\npictrure: {urlPicture}");
                stepsOfRecipe[i] = new StepRecipe(description, urlPicture);
            }


            return new RecipeFull[]
            {
                new RecipeFull(webSite, title, titlePicture,
                    ingredients, stepsOfRecipe, introductionContent, endContent)
            };
        }

        public int GetCount() => 1;
    }
}