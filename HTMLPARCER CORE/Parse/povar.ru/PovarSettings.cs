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

        public string BaseUrl { get; set; } = "https://povar.ru";

        public string Prefix { get; set; } = "mostnew/all/{CurrentId}";

        public int Count { get; set; }
        public string PrefixFind { get; set; } = "xmlsearch?query={CurrentName}";
        public string Recipe { get; set; }
    }
}
