using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class TvoireceptyParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ClassList.Contains("recipe-title")).ToArray();

            count = title.Length;

            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("tvoirecepty.ru", title[j].TextContent.Replace("\n", "").Replace("        ", ""),
                    ("https://tvoirecepty.ru/files/imagecache/recept_teaser" + title[j].Attributes[1].Value + ".jpg").Replace("recept/", "recept/recept-"),
                    "https://tvoirecepty.ru" + title[j].Attributes[1].Value));

            return list.ToArray();
        }
    }
}
