using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLPARCER_CORE.Parse
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }
        string Prefix { get; set; }
        string PrefixFind { get; set; }
        string Recipe { get; set; }
        int MinPage { get; set; }
        int MaxPage { get; set; }

        int MaxCountPage { get; set; }
    }
}