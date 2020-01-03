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
                    && item.Attributes[0].Value != "https://eda.ru/specialproject/gold_1000").ToArray() ;
            
            var urlPicture = document.QuerySelectorAll("svg").
                Where(item => item.ClassList != null && (
                item.ClassList.Contains("horizontal-tile__preview-image") || )).ToArray();

            count = title.Length;
            System.Console.WriteLine(title.Length);
            System.Console.WriteLine(urlPicture.Length);
            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort(
                    "eda.ru",
                    title[j].TextContent.Replace("  ","").Replace("\n", ""),
                    urlPicture[j].Attributes[3].Value,
                    "https://eda.ru" + title[j].Attributes[0].Value));

            return list.ToArray();
        }
    }
}
