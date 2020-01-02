using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class EdaParser : IParser<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ParentElement.TagName == "H3" && item.ParentElement.ClassList.Contains("item-title")
                    && item.Attributes[0].Value != "https://eda.ru/recepty/afishaeda" 
                    && item.Attributes[0].Value != "https://eda.ru/specialproject/gold_1000").ToArray() ;
            
            var urlPicture = document.QuerySelectorAll("div").
                Where(item => item.ClassList != null && 
                item.ClassList.Contains("lazy-load-container")).ToArray();

            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort(
                    "eda.ru",
                    urlPicture[j].Attributes[2].Value,
                    urlPicture[j].Attributes[3].Value,
                    urlPicture[j].Attributes[2].Value,
                    "https://eda.ru" + title[j].Attributes[0].Value));

            return list.ToArray();
        }
    }
}
