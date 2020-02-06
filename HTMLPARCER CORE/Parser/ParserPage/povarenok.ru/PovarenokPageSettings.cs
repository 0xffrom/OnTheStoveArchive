using RecipeLibrary.Parse;

namespace RecipeLibrary.ParsePage
{
    internal class PovarenokPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.povarenok.ru/recipes/~{PageId}/";
        public string SuffixNew { get; set; } = "?sort=new&order=desc";
        public string SuffixPopular { get; set; } = "?sort=rating&order=desc";
        public string SuffixName { get; set; } = "?name={RecipeName}";
        public int MaxPageId { get; set; } = 0; // TODO: Определить количество.

        public int PageId { get; set; }
        public string RecipeName { get; }



        PovarenokPageSettings(int pageId)
        {
            PageId = pageId;
        }

        PovarenokPageSettings(int pageId, string recipeName) : this(pageId)
        {
            RecipeName = recipeName;
        }
    }
}