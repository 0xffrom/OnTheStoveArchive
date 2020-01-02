using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarSettings : IParserSettings
    {
        public PovarSettings(int count)
        {
            Count = count;
        }

        public PovarSettings()
        {
            Count = 1;
        }

        public string BaseUrl { get; set; } = "https://povar.ru/mostnew/all";

        public string Prefix { get; set; } = "{CurrentId}";

        public int Count { get; set; }
        public string PrefixFind { get; set; }
        public string Recipe { get; set; }
    }
}
