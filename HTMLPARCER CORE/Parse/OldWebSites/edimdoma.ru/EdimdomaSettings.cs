/*using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class EdimdomaSettings : IParserSettings
    {
        public EdimdomaSettings(int count)
        {
            MaxPage = count;
        }

        public EdimdomaSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public EdimdomaSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public EdimdomaSettings()
        {
            MaxPage = 1;
        }

        public EdimdomaSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public EdimdomaSettings(string recipe, int count) : this(recipe)
        {
            MaxPage = count;
        }

        public string BaseUrl { get; set; } = "https://www.edimdoma.ru";

        public string Prefix { get; set; } = "retsepty?page={CurrentId}";
        public string PrefixFind { get; set; } = "retsepty?direction=&field=&page={CurrentId}&query=";
        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; }
        public int MaxCountPage { get; set; }
    }
}
*/