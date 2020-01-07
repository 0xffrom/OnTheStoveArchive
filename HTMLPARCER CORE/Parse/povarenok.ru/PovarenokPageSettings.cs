using HTMLPARCER_CORE.Parse;

namespace HTMLPARCER_CORE
{
    public class PovarenokPageSettings : IParserSettings
    {
        public PovarenokPageSettings(string url)
        {
            BaseUrl = url;
        }
        
        public string BaseUrl { get; set; }
        public string Prefix { get; set; } = null;
        public string PrefixFind { get; set; } = null;
        public string Recipe { get; set; } = null;
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 1;
        public int MaxCountPage { get; set; } = 1;
    }
}