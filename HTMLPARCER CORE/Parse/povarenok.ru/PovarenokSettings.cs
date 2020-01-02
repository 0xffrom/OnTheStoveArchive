using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarenokSettings : IParserSettings
    {
        public PovarenokSettings(int count)
        {
            MaxPage = count;
        }

        public PovarenokSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public PovarenokSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public PovarenokSettings()
        {
            MaxPage = 1;
        }

        public PovarenokSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public PovarenokSettings(string recipe, int count) : this(recipe)
        {
            MaxPage = count;
        }

        public string BaseUrl { get; set; } = "https://www.povarenok.ru/recipes";

        public string Prefix { get; set; } = "~{CurrentId}";
        public string PrefixFind { get; set; } = "search/?name={CurrentName}#searchformtop";
        public string PrefixFindWithCount { get; set; } = "";
        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 0;

    }
}
