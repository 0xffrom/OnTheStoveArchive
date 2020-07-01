using System.Collections.Generic;
namespace ObjectsLibrary.Parser.ParserPage.Core
{
    public interface IParserPageSettings
    {
        string Url { get; }
        string Section { get; }
        Dictionary<string, string> Sections { get; }
        int MaxPageId { get; }
        int PageId { get; }
        double IndexPopularity { get; set; }
        double IndexStep { get; }
        string RecipeName { get; }
    }
}