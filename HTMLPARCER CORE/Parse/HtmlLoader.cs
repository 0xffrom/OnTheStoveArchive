using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTMLPARCER_CORE.Parse
{
    public class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseUrl}/{settings.Prefix}/";
        }

        public async Task<string> GetSourceByPageId(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());
            var response = await client.GetAsync(currentUrl);
            string source = String.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }
    }
}
