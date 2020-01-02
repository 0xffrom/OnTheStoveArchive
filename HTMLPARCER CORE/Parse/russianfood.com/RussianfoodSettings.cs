using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class RussianfoodSettings : IParserSettings
    {
        public RussianfoodSettings(int count)
        {
            Count = count;
        }
        public RussianfoodSettings()
        {
            Count = 1;
        }
        public RussianfoodSettings(string recipe)
        {
            Recipe = recipe;
        }
        public string BaseUrl { get; set; } = "https://www.russianfood.com/recipes/bytype";

        public string Prefix { get; set; } = "?fid=791&sort=id&page={CurrentId}";

        public int Count { get; set; }
        public string PrefixFind { get; set; }
        public string Recipe { get; set; }
    }
}
