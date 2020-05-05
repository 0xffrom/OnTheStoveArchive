using System.Collections.Generic;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    class EdaPageSettings : IParserPageSettings
    {
        public string Url { get; } = "https://eda.ru/";
        public string Section { get; }

        public Dictionary<string, string> Sections { get; } = new Dictionary<string, string>()
        {
            {"new", "RecipesCatalog/GetPage?sorting=date&page={PageId}"},
            {"random", "RecipesCatalog/GetPage?sorting=rate&page={PageId}"},
            {"popular", "RecipesCatalog/GetPage?sorting=rating&page={PageId}"},
            {"recipe", "RecipesSearch/GetNextRecipes?sorting=rating&page={PageId}&q={RecipeName}&OnlyEdaChecked=false"},
            {"горячее", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=osnovnye-blyuda"},
            {"супы", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=supy"},
            {"салаты", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=salaty"},
            {"закуски", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=zakuski"},
            {"выпечка", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=vypechka-deserty"},
            {"десерты", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=vypechka-deserty"},
            {"соусы", "RecipesCatalog/GetPage?sorting=rate&page={PageId}&tags=sousy-marinady"}
        };

        public int MaxPageId { get; } = 350;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 100;
        public double IndexStep { get; } = 1;

        public EdaPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public EdaPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = recipeName.Replace(' ', '+');
        }
    }
}