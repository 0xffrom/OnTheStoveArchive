using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarSettings : IParserSettings
    {
        public PovarSettings(int count)
        {
            Count = count;
        }

        public PovarSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public PovarSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public PovarSettings()
        {
            Count = 1;
        }

        public PovarSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public PovarSettings(string recipe, int count) : this(recipe)
        {
            Count = count;
        }

        public string BaseUrl { get; set; } = "https://povar.ru";

        public string Prefix { get; set; } = "mostnew/all/{CurrentId}";

        public int Count { get; set; }
        public string PrefixFind { get; set; } = "xmlsearch?query=";
        public string Recipe { get; set; }
        public string PrefixFindWithCount { get; set; } = "&page={CurrentId}";
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; }
    }
}
