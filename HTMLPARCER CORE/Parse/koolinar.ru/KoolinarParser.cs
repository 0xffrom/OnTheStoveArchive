using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class KoolinarParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ClassList.Contains("g-color-main g-color-primary--hover")).ToArray();

            var urlPicture = document.QuerySelectorAll("img").
                Where(item => item.ClassList != null &&
                item.ClassList.Contains("img-fluid w-100 u-block-hover__main--zoom-v1")).ToArray();

            count = title.Length;

            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("koolinar.ru", title[j].FirstElementChild.TextContent, "https://koolinar.ru" + urlPicture[j].Attributes[2].Value,
                     "https://koolinar.ru" + title[j].Attributes[2].Value));

            return list.ToArray();
        }
    }
}
