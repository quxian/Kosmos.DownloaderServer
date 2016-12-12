using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.DownloaderServer.Controllers
{
    public class DownloaderController : ApiController
    {
        [HttpGet]
        [Route("api/Downloader/AddProcesserServersAddress")]
        public async Task<IHttpActionResult> AddProcesserServersAddress(string address)
        {
            if (ProcesserServersAddressCache.Urls.IndexOf(address) >= 0)
                return Ok(address);

            ProcesserServersAddressCache.Urls.Add(address);
            return Ok(address);
        }

        [HttpPost]
        [Route("api/Downloader/AddProcesserServersAddress")]
        public async Task<IHttpActionResult> AddProcesserServersAddress(List<string> address)
        {
            var newAddress = address.Except(ProcesserServersAddressCache.Urls);

            ProcesserServersAddressCache.Urls.AddRange(newAddress);
            return Ok(newAddress);
        }

        [HttpGet]
        [Route("api/Downloader/ProcesserServerAddress/Delete")]
        public async Task<IHttpActionResult> DeleteProcesserServerAddress(string address)
        {
            ProcesserServersAddressCache.Urls.Remove(address);
            return Ok(address);
        }

        [HttpGet]
        [Route("api/Downloader/ProcesserServerAddress/List")]
        public async Task<IHttpActionResult> ListProcesserServerAddress()
        {
            return Ok(ProcesserServersAddressCache.Urls);
        }
    }
}
