using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class EdaSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://eda.ru";
        public string Prefix { get; set; } = "recepty?page={CurrentId}";
        public string PrefixFind { get; set; } = "recipesearch?page=1&q=";
        public string Recipe { get; set; } = null;
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 1;
        public int MaxCountPage { get; set; }

        // Сайт https://eda.ru не поддерживает поиск конкретного рецепта по нескольким страницам.

        public EdaSettings(int count)
        {
            MaxPage = count;
        }

        public EdaSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }


        public EdaSettings()
        {
            MaxPage = 1;
        }


    }
}
