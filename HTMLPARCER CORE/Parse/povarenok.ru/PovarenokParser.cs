using AngleSharp.Html.Dom;
using HTMLPARCER_CORE.Parse;
using System.Collections.Generic;
using System.Linq;

namespace HTMLPARCER_CORE
{
    public class PovarenokParser : IParser<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var list = new List<RecipeShort>();

            var title = document.QuerySelectorAll("a").
                    Where(item => item.ClassList != null && item.ParentElement.TagName == "H2" &&
                    item.ParentElement.ParentElement.ClassName.Contains("item-bl")).ToArray();

            var urlPicture = document.QuerySelectorAll("img").
                Where(item => item.ClassList != null && item.ParentElement.ParentElement.TagName == "DIV" &&
                item.ParentElement.ParentElement.ClassName.Contains("m-img conima")).ToArray();


            for (int j = 0; j < title.Length; j++)
                list.Add(new RecipeShort("povarenok.ru", title[j].TextContent, urlPicture[j].Attributes[0].Value.Replace("-330x220x", ""),
                     title[j].Attributes[0].Value));

            return list.ToArray();
        }
    }
}
