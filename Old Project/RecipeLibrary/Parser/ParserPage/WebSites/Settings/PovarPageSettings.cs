using System.Collections.Generic;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    internal class PovarPageSettings : IParserPageSettings
    {
        public string Url { get; } = "https://povar.ru/";
        public string Section { get; }

        public Dictionary<string, string> Sections { get; } = new Dictionary<string, string>()
        {
            {"new", "mostnew/all/{PageId}/"},
            {"random", "mostnew/all/{PageId}/"},
            {"popular", "master/rating/all/{PageId}/"},
            {"recipe", "xmlsearch?query={RecipeName}&page={PageId}"},
            {"горячее", "master/goryachie_bliuda/{PageId}/"},
            {"супы", "master/soup/{PageId}/"},
            {"салаты", "master/salad/{PageId}/"},
            {"закуски", "master/zakuski/{PageId}/"},
            {"выпечка", "master/vypechka/{PageId}/"},
            {"десерты", "master/dessert/{PageId}/"},
            {"соусы", "master/sousy/{PageId}/"}
        };

        public int MaxPageId { get; set; } = 1788;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 100;
        public double IndexStep { get; } = 1;

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