using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarenokSettings : IParserSettings
    {
        public PovarenokSettings(int count)
        {
            Count = count;
        }
        public PovarenokSettings()
        {
            Count = 1;
        }
        public string BaseUrl { get; set; } = "https://www.povarenok.ru/recipes";

        public string Prefix { get; set; } = "~{CurrentId}";

        public int Count { get; set; }
        public string PrefixFind { get; set; } = "search/?name={CurrentName}#searchformtop";
        public string Recipe { get; set; }
        public string PrefixFindWithCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MinPage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxPage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
