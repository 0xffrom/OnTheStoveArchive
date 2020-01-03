using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class EdimdomaParser : IParser<RecipeShort[]>
    {
        public static int count;

        public int GetCount() => count;

        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null &&
                    item.ParentElement.ClassList.Contains("card__description")).ToArray();

            var urlPicture = document.QuerySelectorAll("img").
                Where(item => item.ClassList != null  &&
                item.ParentElement.ClassList.Contains("card__picture")).ToArray();

            count = title.Length;

            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("edimdoma.ru", title[j].FirstElementChild.TextContent, urlPicture[j].Attributes[2].Value,
                     "https://povar.ru" + title[j].Attributes[0].Value));

            return list.ToArray();
        }
    }
}
