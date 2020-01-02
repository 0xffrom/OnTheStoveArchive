using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class EdaSettings : IParserSettings
    {
        public EdaSettings(int count)
        {
            Count = count;
        }

        public EdaSettings()
        {
            Count = 1;
        }
        public EdaSettings(string recipe)
        {
            Recipe = recipe;
        }
        public EdaSettings(string recipe, int count) : this(recipe)
        {
            Count = count;
        }

        public string BaseUrl { get; set; } = "https://eda.ru";
        public string Prefix { get; set; } = "recepty?page={CurrentId}";
        public int Count { get; set; }
        public string PrefixFind { get; set; } = "recipesearch?q=";
        public string Recipe { get; set; }
    }
}
