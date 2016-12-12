using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kosmos.DownloaderServer
{
    public static class ProcesserServersAddressCache
    {
        public static List<string> Urls { get; set; }
        static ProcesserServersAddressCache() {
            Urls = new List<string>();
        }
    }
}