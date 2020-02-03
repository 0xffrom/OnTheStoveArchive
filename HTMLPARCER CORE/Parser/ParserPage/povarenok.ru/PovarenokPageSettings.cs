using RecipeLibrary.Parse;
namespace povarenok.ru
{
    internal class PovarenokPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.povarenok.ru/recipes/~{PageId}/";
        public string SuffixNew { get; set; } = "?sort=new&order=desc";
        public string SuffixPopular { get; set; } = "?sort=rating&order=desc";
        public string SuffixName { get; set; } = "?name={RecipeName}";
        public int MaxPageId { get; set; } = 0; // TODO: Определить количество.
    }