using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.povarenok.ru
{
    internal class PovarenokPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.povarenok.ru/recipes/~{PageId}/";
        public string SuffixNew { get; set; } = "?sort=new&order=desc";
        public string SuffixPopular { get; set; } = "?sort=rating&order=desc";
        public string SuffixName { get; set; } = "?name={RecipeName}";
        public string Section { get; set; }
        public int MaxPageId { get; set; } = 0; // TODO: Определить количество.
        public int PageId { get; set; }
        public string RecipeName { get; }


        public PovarenokPageSettings(string section, int pageId, string recipeName)
        {
            Section = section;
            PageId = pageId;
            RecipeName = recipeName;
        }

    }
}