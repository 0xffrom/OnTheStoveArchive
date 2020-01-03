using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class EdaParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ParentElement.TagName == "H3" && item.ParentElement.ClassList.Contains("item-title")
                    && item.Attributes[0].Value != "https://eda.ru/recepty/afishaeda" 
                    && item.Attributes[0].Value != "https://eda.ru/specialproject/gold_1000"
                    && !item.ParentElement.ParentElement.ParentElement.
                    FirstElementChild.FirstElementChild.LastElementChild.
                    ClassList.Contains("horizontal-tile__preview-image_empty")).ToArray() ;
            
            var urlPicture = document.QuerySelectorAll("div").
                Where(item => item.ClassList != null && (
                item.ClassList.Contains("lazy-load-container"))).ToArray();

            
          
            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort(
                    "eda.ru",
                    title[j].TextContent.Replace("  ","").Replace("\n", ""),
                    urlPicture[j].Attributes[3].Value,
                    "https://eda.ru" + title[j].Attributes[0].Value));

            // Обработка странички, на которой все рецепты без фотографии.
            if (title.Length == 0)
            {
                title = document.QuerySelectorAll("a").
                 Where(item => item.ClassList != null &&
                 item.ParentElement.TagName == "H3" && item.ParentElement.ClassList.Contains("item-title")
                 && item.Attributes[0].Value != "https://eda.ru/recepty/afishaeda"
                 && item.Attributes[0].Value != "https://eda.ru/specialproject/gold_1000").ToArray();
                if (title.Length != 0)
                {
                    list.Add(new RecipeShort(
                      "eda.ru",
                      title[0].TextContent.Replace("  ", "").Replace("\n", ""),
                      "//s2.eda.ru/StaticContent/DefaultRecipePhoto/no-photo.svg",
                      "https://eda.ru" + title[0].Attributes[0].Value));
                }

            }

            count = list.Count;
            return list.ToArray();
        }
    }
}
