/*
using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class RussianfoodParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ParentElement.ClassList.Contains("title")).ToArray();

            var urlPicture = document.QuerySelectorAll("img").
                Where(item => item.ClassList != null && item.ParentElement.ParentElement.ClassList.Contains("foto") &&
                item.ClassList.Contains("shadow")).ToArray();

            count = title.Length;

            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("russianfood.com", title[j].TextContent, urlPicture[j].Attributes[1].Value,
                    "https://www.russianfood.com" + title[j].Attributes[1].Value));

            return list.ToArray();
        }
    }
}
*/