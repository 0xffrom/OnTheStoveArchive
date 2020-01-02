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
            if (string.IsNullOrEmpty(settings.Recipe))
                url = $"{settings.BaseUrl}/{settings.Prefix}/";
            else
            {
                if (settings.MaxPage != 0)
                    url = $"{settings.BaseUrl}/{settings.PrefixFind}{settings.Recipe}{settings.PrefixFindWithCount}/";
                else
                    url = $"{settings.BaseUrl}/{settings.PrefixFind}{settings.Recipe}/";
            }
               
            

        }

        public async Task<string> GetSource(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());
            Console.WriteLine(currentUrl);
            var response = await client.GetAsync(currentUrl);
            string source = String.Empty;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();

            return source;
        }
    }
}
