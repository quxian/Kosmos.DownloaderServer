using Kosmos.DownloaderServer.DbContext;
using Kosmos.DownloaderServer.Model;
using Kosmos.Singleton;
using Newtonsoft.Json;
using StringExtensionForYongsheng;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.DownloaderServer.Controllers
{
    public class DownloadController : ApiController
    {

        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly DateTime _dateTime = DateTime.Now;

        public DownloadController(AppDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 下载指定的Url内容
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>下载结果</returns>
        public async Task<IHttpActionResult> Get(Models.Url url)
        {
            return await DoDownload(url);
        }

        private async Task<IHttpActionResult> DoDownload(Models.Url url)
        {
            try
            {
                var result = "";
                try
                {
                    result = await _httpClient.GetStringAsync(url.Value);
                }
                catch (Exception e)
                {
                    SingleHttpClient.PostException(e);

                    return BadRequest(e.Message);
                }
                var resultHashCode = result.GetMD5HashCode();

                var downloadedResult = new DownloadedResult
                {
                    Depth = url.Depth,
                    Domain = new Uri(url.Value).Host,
                    DownloadDate = _dateTime,
                    IsExtracted = false,
                    Result = result,
                    ResultHashCode = resultHashCode,
                    Url = url.Value
                };

                ResultCahce.Results.TryAdd(resultHashCode, downloadedResult);

                var task = Task.Run(async () =>
                {
                    try
                    {
                        var httpResponseMessage = await _httpClient.PostAsJsonAsync($"{ProcesserServersAddressCache.Urls.First()}api/Process", downloadedResult);
                        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            downloadedResult.IsExtracted = true;
                            ResultCahce.Results.TryUpdate(downloadedResult.ResultHashCode, downloadedResult, downloadedResult);
                        }
                    }
                    catch (Exception e)
                    {
                        SingleHttpClient.PostException(e);
                    }
                });

                return Ok();
            }
            catch (Exception e)
            {
                SingleHttpClient.PostException(e);

                return BadRequest(url.Value);
            }
        }

        [HttpPost]
        [Route("api/Download")]
        public async Task<IHttpActionResult> Post(Models.Url url)
        {
            return await DoDownload(url);
        }

        [HttpGet]
        [Route("api/Download/CacheToDb")]
        public async Task<IHttpActionResult> CacheToDb()
        {
            using (var dbContext = new AppDbContext())
            {
                ResultCahce.CacheToDb(dbContext);
            }

            return Ok();
        }
    }
}
