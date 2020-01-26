/*
using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class VkusoSettings : IParserSettings
    {
        public VkusoSettings(int count)
        {
            MaxPage = count;
        }

        public VkusoSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public VkusoSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = recipe;
        }

        public VkusoSettings()
        {
            MaxPage = 1;
        }

        public VkusoSettings(string recipe) : this()
        {
            Recipe = recipe;
        }

        public VkusoSettings(string recipe, int count) : this(recipe)
        {
            MaxPage = count;
        }

        public string BaseUrl { get; set; } = "https://vkuso.ru/recipes/page";

        public string Prefix { get; set; } = "{CurrentId}";
        public string PrefixFind { get; set; } = "{CurrentId}/?s=";
        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; }
        public int MaxCountPage { get; set; }
    }
}
*/