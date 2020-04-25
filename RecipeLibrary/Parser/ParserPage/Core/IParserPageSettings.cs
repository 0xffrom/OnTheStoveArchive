using System.Collections.Generic;
namespace ObjectsLibrary.Parser.ParserPage.Core
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
        double IndexPopularity { get; set; }
        double IndexStep { get; set; }
        
        Dictionary<string, string> Sections { get; set; }
        string RecipeName { get; }
    }
}