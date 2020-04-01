using System.Collections.Generic;
using System.Linq;
using ObjectsLibrary.Parser.ParserPage.Core;

namespace ObjectsLibrary.Parser.ParserPage.WebSites
{
    internal class PovarenokPageSettings : IParserPageSettings
    {
        public string Url { get; set; } = "https://www.povarenok.ru/recipes/";
        public string SuffixNew { get; set; } = "~{PageId}/?sort=new&order=desc";
        public string SuffixPopular { get; set; } = "~{PageId}/?sort=rating&order=desc";
        public string SuffixRecipe { get; set; } = "search/~{PageId}/?name={RecipeName}#searchformtop";
        public string Section { get; set; }
        public int MaxPageId { get; set; } = 9234;
        public int PageId { get; set; }
        public string RecipeName { get; }
        public double IndexPopularity { get; set; } = 95;
        public double IndexStep { get; set; } = 2;


        public PovarenokPageSettings(string section, int pageId)
        {
            Section = section;
            PageId = pageId;
        }

        public PovarenokPageSettings(string section, int pageId, string recipeName) : this(section, pageId)
        {
            RecipeName = GetUrl(recipeName);
        }

        private string GetUrl(string recipeName)
        {
            // абвгдеёжзийклмнопрстуфхцчшщъыьэюя =>
            // %E0%E1%E2%E3%E4%E5%B8%E6%E7%E8%E9%EA%EB%EC%ED%EE%EF%F0%F1%F2%F3%F4%F5%F6%F7%F8%F9%FA%FB%FC%FD%FE%FF

            var valuePairs = new Dictionary<char, string>()
            {
                {'a', "E0"}, {'б', "E1"}, {'в', "E2"}, {'г', "E3"}, {'д', "E4"}, {'е', "E5"},
                {'ё', "B8"}, {'ж', "E6"}, {'з', "E7"}, {'и', "E8"}, {'й', "E9"}, {'к', "EA"},
                {'л', "EB"}, {'м', "EC"}, {'н', "ED"}, {'о', "EE"}, {'п', "EF"}, {'р', "F0"},
                {'с', "F1"}, {'т', "F2"}, {'у', "F3"}, {'ф', "F4"}, {'х', "F5"}, {'ц', "F6"},
                {'ч', "F7"}, {'ш', "F8"}, {'щ', "F9"}, {'ъ', "FA"}, {'ы', "FB"}, {'ь', "FC"},
                {'э', "FD"}, {'ю', "FE"}, {'я', "FF"}
            };

            string url = string.Empty;

            foreach (char symb in recipeName)
                url += '%' + valuePairs.FirstOrDefault(item => item.Key == symb).Value;
            return url;
        }

    }
}