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
        private string endContentText;
        private string[] endContentPictures;

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
                item.ClassName != null &&
                item.ClassName.Contains("ingredients-bl")).ToArray();
            

            var stepsOfRecipeArray = document.QuerySelectorAll("div")
                .Where(item => item.ClassList != null && item.ClassList.Contains("cooking-bl")).ToArray();


            var endContentArray = document.QuerySelectorAll("div").Where(item =>
                    item.ParentElement.ParentElement.ClassName != null &&
                    item.ClassName == null &&
                    item.ParentElement.ParentElement.ClassName.Contains("item-bl item-about")).ToArray();

            //TODO: Сделать специальный массив с пикчами, а endcontent всего лишь текст.

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
                        int index = text.IndexOf("—");
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
                stepsOfRecipe[i] = new StepRecipe(description, urlPicture);
            }

            endContentText = endContentArray[0].TextContent;

            var endContentPicturesArray = endContentArray[0].QuerySelectorAll("img")
                .Where(item => item.ClassName != null && item.ClassName.Contains("bbimg")).ToArray();

            endContentPictures = new string[endContentPicturesArray.Length];

            for (int i = 0; i < endContentPicturesArray.Length; i++)
            {
                endContentPictures[i] = "https://www.povarenok.ru/" + endContentPicturesArray[i].Attributes[3].Value;   
            }

            return new RecipeFull[]
            {
                new RecipeFull(webSite, title, titlePicture,
                    ingredients, stepsOfRecipe, introductionContent, endContentText, endContentPictures)
            };
        }

        public int GetCount() => 1;
    }
}