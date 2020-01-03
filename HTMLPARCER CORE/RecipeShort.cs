using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLPARCER_CORE
{
    public class RecipeShort
    {
        public string WebSite { get; }
        public string Title { get; }
        public string UrlPicture { get; }
        public string Url { get; }

        public RecipeShort(string webSite, string title, string urlPicture, string url)
        {
            WebSite = webSite;
            Title = title;
            UrlPicture = urlPicture;
            Url = url;
        }
    }
}
