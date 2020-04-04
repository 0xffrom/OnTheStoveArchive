using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    internal class EdimDomaPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.edimdoma.ru/";
        public string SuffixNew { get; set; } = "retsepty?page={PageId}";
        public string SuffixPopular { get; set; } = "retsepty?with_ingredient=&without_ingredient=&user_ids=&page={PageId}&field=popular&direction=desc&query=";
        public string SuffixRecipe { get; set; } = "retsepty?with_ingredient=&without_ingredient=&user_ids=&page={PageId}&field=&direction=&query={RecipeName}";
        public string Section { get; set; }
        public int MaxPageId { get; set; } = 4672;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 100;
        public double IndexStep { get; set; } = 2;

        public EdimDomaPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public EdimDomaPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = recipeName;
        }


    }
}