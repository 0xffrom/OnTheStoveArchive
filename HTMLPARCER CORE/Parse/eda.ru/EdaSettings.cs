using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class EdaSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://eda.ru";
        public string Prefix { get; set; } = "recepty?page={CurrentId}";
        public int Count { get; set; }
        public string PrefixFind { get; set; } = "recipesearch?q=";
        public string PrefixFindWithCount { get; set; } = "&page={CurrentId}";
        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 1;

        // Сайт https://eda.ru не поддерживает поиск конкретного рецепта по нескольким страницам.

        public EdaSettings(int count)
        {
            Count = count;
        }

        public EdaSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public EdaSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public EdaSettings()
        {
            Count = 1;
        }

        public EdaSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public EdaSettings(string recipe, int count) : this(recipe)
        {
            Count = count;
        }

        
    }
}
