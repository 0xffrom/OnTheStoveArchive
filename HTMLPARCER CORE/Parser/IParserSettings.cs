
namespace RecipeLibrary.Parse
{
    internal interface IParserSettings
    {
        string Url { get; set; }
        string Suffix { get; set; }

        int MinPage { get; set; }
        int MaxPage { get; set; }
        int MaxCountPage { get; set; }
    }
}