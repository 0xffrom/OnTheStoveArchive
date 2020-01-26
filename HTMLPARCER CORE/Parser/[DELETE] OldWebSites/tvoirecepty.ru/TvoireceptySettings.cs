/*
using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class TvoireceptySettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://tvoirecepty.ru";
        public string Prefix { get; set; } = "recepty?page={CurrentId}";
        public string PrefixFind { get; set; } = "search/apachesolr_search/";
        public string Recipe { get; set; } = null;
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 1;
        public int MaxCountPage { get; set; } = 102;
        // Максимальная страница вводится вручную, так как сайт выдаёт одни и те же рецепты
        // на следующих страницах после максимальной.

        // Сайт https://tvoirecepty.ru не поддерживает поиск конкретного рецепта по нескольким страницам.

        public TvoireceptySettings(int count)
        {
            MaxPage = count;
        }

        public TvoireceptySettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }


        public TvoireceptySettings()
        {
            MaxPage = 1;
        }


    }
}
*/