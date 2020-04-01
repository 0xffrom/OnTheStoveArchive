using RecipeLibrary.Parser.ParserPage.Core;
using System.Collections.Generic;
using System.Linq;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    internal class PovarPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://povar.ru/";
        public string SuffixNew { get; set; } = "mostnew/all/{PageId}/";
        public string SuffixPopular { get; set; } = "master/rating/all/{PageId}/";
        public string SuffixRecipe { get; set; } = "xmlsearch?query={RecipeName}&page={PageId}";
        public string Section { get; set; }
        public int MaxPageId { get; set; } = 1788;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 100;
        public double IndexStep { get; set; } = 1;


        public PovarPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public PovarPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = recipeName;
        }
        

    }
}