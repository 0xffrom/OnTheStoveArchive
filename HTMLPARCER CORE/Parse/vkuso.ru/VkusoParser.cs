using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class VkosoParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null && item.ParentElement.TagName == "H3" &&
                    item.ParentElement.ParentElement.ClassName.Contains("item__line_content")).ToArray();

            var urlPicture = document.QuerySelectorAll("img").
                Where(item => item.ClassList != null &&
                item.ParentElement.ClassList.Contains("card__image")).ToArray();

            count = title.Length;
            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("vkuso.ru", title[j].TextContent, urlPicture[j].Attributes[2].Value,
                    title[j].Attributes[0].Value));

            return list.ToArray();
        }
    }
}
