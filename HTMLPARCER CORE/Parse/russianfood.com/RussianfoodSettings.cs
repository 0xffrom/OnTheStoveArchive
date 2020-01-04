using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class RussianfoodSettings : IParserSettings
    {
        public RussianfoodSettings(int count)
        {
            MaxPage = count;
        }

        public RussianfoodSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }


        public RussianfoodSettings()
        {
            MaxPage = 1;
        }


        public string BaseUrl { get; set; } = "https://www.russianfood.com/recipes/bytype";
        public string Prefix { get; set; } = "?fid=791&sort=id&page={CurrentId}";
        public int Count { get; set; }
        public string PrefixFind { get; set; }
        public string Recipe { get; set; } = null;
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 0;
        public int MaxCountPage { get; set; }
    }
}
