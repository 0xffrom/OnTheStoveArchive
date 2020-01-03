using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class KoolinarSettings : IParserSettings
    {
        public KoolinarSettings(int count)
        {
            MaxPage = count;
        }

        public KoolinarSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public KoolinarSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public KoolinarSettings()
        {
            MaxPage = 1;
        }

        public KoolinarSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public KoolinarSettings(string recipe, int count) : this(recipe)
        {
            MaxPage = count;
        }

        public string BaseUrl { get; set; } = "https://www.koolinar.ru";

        public string Prefix { get; set; } = "recipes?page={CurrentId}";
        public string PrefixFind { get; set; } = "list?utf8=✓&page={CurrentId}&search_term_string=";
        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; }
    }
}
