using System.Collections.Generic;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    internal class EdimDomaPageSettings : IParserPageSettings
    {
        public string Url { get; } = "https://www.edimdoma.ru/";
        public string Section { get; }

        public Dictionary<string, string> Sections { get; } = new Dictionary<string, string>()
        {
            {"new", "retsepty?page={PageId}"},
            {"random", "retsepty?page={PageId}"},
            {
                "popular",
                "retsepty?with_ingredient=&without_ingredient=&user_ids=&page={PageId}&field=popular&direction=desc"
            },
            {
                "recipe",
                "retsepty?with_ingredient=&without_ingredient=&user_ids=&page={PageId}&field=&direction=&query={RecipeName}"
            },
            {"горячее", "retsepty?page={PageId}&tags[recipe_category][]=основные+блюда"},
            {"супы", "retsepty?page={PageId}&tags[recipe_category][]=супы+и+бульоны"},
            {"салаты", "retsepty?page={PageId}&tags[recipe_category][]=салаты+и+винегреты"},
            {"закуски", "retsepty?page={PageId}&tags[recipe_category][]=закуски"},
            {"выпечка", "retsepty?page={PageId}&tags[recipe_category][]=выпечка"},
            {"десерты", "retsepty?page={PageId}&tags[recipe_category][]=десерты"},
            {"соусы", "retsepty?page={PageId}&tags[recipe_category][]=соусы+и+заправки"}
        };

        public int MaxPageId { get; } = 4672;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 100;
        public double IndexStep { get; } = 1;

        public EdimDomaPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public EdimDomaPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = recipeName.Replace(' ', '+');
        }
    }
}