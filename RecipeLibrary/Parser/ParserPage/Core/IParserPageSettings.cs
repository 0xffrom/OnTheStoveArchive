namespace RecipeLibrary.Parser.ParserPage.Core
{
    public interface IParserPageSettings
    {
        string Url { get; set; }
        string Section { get; set; }
        string SuffixNew { get; set; }
        string SuffixPopular { get; set; }
        string SuffixRecipe { get; set; }
        int MaxPageId { get; set; }
        int PageId { get; set; }
        string RecipeName { get; }
    }
}