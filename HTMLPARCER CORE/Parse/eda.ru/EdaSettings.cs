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

        public string BaseUrl { get; set; } = "https://eda.ru";
        public string Prefix { get; set; } = "recepty?page={CurrentId}";
        public int Count { get; set; }
        public string PrefixFind { get; set; }
        public string Recipe { get; set; }
    }
}
