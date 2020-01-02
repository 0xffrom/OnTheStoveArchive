using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }

        string Prefix { get; set; }


        public string PrefixFind { get; set; }

        public string Recipe { get; set; }

        public string PrefixFindWithCount { get; set; }
        public int MinPage { get; set; }
        public int MaxPage { get; set; }
    }
}
