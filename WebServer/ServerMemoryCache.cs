using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace WebServer
{
    public class ServerMemoryCache
    {
        public MemoryCache Cache { get; set; }
        public ServerMemoryCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1536
            });
        }
    }
}
