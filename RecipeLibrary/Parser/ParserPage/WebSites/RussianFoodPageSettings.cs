using System;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    internal class EdaPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.russianfood.com/";
        public string SuffixNew { get; set; } = "recipes/bytype/?fid=791&sort=id&page={PageId}";
        public string SuffixPopular { get; set; } = "recipes/bytype/?fid=791&page={PageId}";
        public string SuffixRecipe { get; set; } = " ";
        public string Section { get; set; }
        public int MaxPageId { get; set; }
        public int PageId { get; set; }
        public string RecipeName { get; }


        public EdaPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public EdaPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = recipeName;
            
            // TODO: Дописать количество страниц и придумать парсинг отдельных страниц с рецептами. Популяр и нью настроены.
        }
        

    }
}