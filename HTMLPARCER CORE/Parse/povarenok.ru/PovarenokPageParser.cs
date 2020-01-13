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
        private string url;
        private string webSite = "povarenok.ru";
        private string title;
        private string titlePicture;
        private Ingredient[] ingredients;
        private StepRecipe[] stepsOfRecipe;
        private string introductionContent;
        private string endContentText;
        private string[] endContentPictures;
        private string[] contentVideos;

        public RecipeFull[] Parse(IHtmlDocument document)
        {
            
            url = document.QuerySelectorAll("meta").
                Where(item => item.Attributes[0]!= null && item.Attributes[0].Value == "og:url").
                ToArray()[0].Attributes[1].Value;

            title = document.QuerySelectorAll("meta")
               .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:title")).ToArray()[0]
               .Attributes[1].Value;

            titlePicture = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("og:image")).ToArray()[0]
                .Attributes[1].Value;

            introductionContent = document.QuerySelectorAll("meta")
                .Where(item => item.Attributes[0] != null && item.Attributes[0].Value.Contains("twitter:description")).ToArray()[0]
                .Attributes[1].Value;

            var ingridientsArray = document.QuerySelectorAll("div").Where(item =>
                item.ClassName != null &&
                item.ClassName.Contains("ingredients-bl")).ToArray();
            

            var stepsOfRecipeArray = document.QuerySelectorAll("div")
                .Where(item => item.ClassList != null && item.ClassList.Contains("cooking-bl")).ToArray();


            var endContentArray = document.QuerySelectorAll("div").Where(item =>
                    item.ParentElement.ParentElement.ClassName != null &&
                    item.ClassName == null &&
                    item.Attributes[0] == null &&
                    item.ParentElement.ParentElement.ClassName.Contains("item-bl item-about")).ToArray();

            var videoArray = document.QuerySelectorAll("div").Where(item => item.ClassName != null &&
            item.ClassName.Contains("video-wrapper")).ToArray();

            List<string> contentVideosList = new List<string>();

            for (int i = 0; i < videoArray.Length; i++)
            {
                contentVideosList.Add(videoArray[i].FirstElementChild.Attributes[2].Value);
            }

            contentVideos = contentVideosList.ToArray();

            var ulIngridientsArray = ingridientsArray[0].QuerySelectorAll("ul").ToArray();

            bool isFragmented = true;

            var pIngridientsArray = ingridientsArray[0].QuerySelectorAll("p").
                Where(item => !item.TextContent.Contains("Время приготовления:") &&
                !item.TextContent.Contains("Количество порций:")).ToArray();

            if (pIngridientsArray.Length == 0)
            {
                isFragmented = false;   
            }

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
                    if(isFragmented)
                        ingridientsList.Add(new Ingredient(pIngridientsArray[i].TextContent, name, unit));
                    else
                        ingridientsList.Add(new Ingredient(title, name, unit));
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

            if (endContentArray != null)
            {
                endContentText = endContentArray[0].TextContent.Replace("  ", "").Replace("\n", "");

                
                var endContentPicturesArray = endContentArray[0].QuerySelectorAll("img")
                    .Where(item => item.ClassName != null && item.ClassName.Contains("bbimg")).ToArray();


                endContentPictures = new string[endContentPicturesArray.Length];

                for (int i = 0; i < endContentPicturesArray.Length; i++)
                {
                    endContentPictures[i] = "https://www.povarenok.ru/" + endContentPicturesArray[i].Attributes[3].Value;
                }
            }
            return new RecipeFull[]
            {
                new RecipeFull(url, webSite, title, titlePicture,
                    ingredients, stepsOfRecipe, introductionContent, endContentText, endContentPictures,contentVideos)
            };
        }

        public int GetCount() => 1;
    }
}