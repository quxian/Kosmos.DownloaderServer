using Extended;
using Kosmos.DownloaderServer.DbContext;
using Kosmos.DownloaderServer.Model;
using Newtonsoft.Json;
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

namespace Kosmos.DownloaderServer.Controllers {
    public class DownloadController : ApiController {

        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly DateTime _dateTime = DateTime.Now;

        public DownloadController(AppDbContext dbContext, HttpClient httpClient) {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 下载指定的Url内容
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>下载结果</returns>
        public async Task<IHttpActionResult> Get(string url) {
            try {
                var result = await _httpClient.GetStringAsync(url);
                var resultHashCode = result.GetMD5HashCode();

                var downloadedResult = await _dbContext.DownloadedResults.FindAsync(resultHashCode);
                if (await _dbContext.DownloadedResults.FindAsync(resultHashCode) != null) {
                    return Ok();
                }

                downloadedResult = new DownloadedResult {
                    Domain = new Uri(url).Host,
                    DownloadDate = _dateTime,
                    IsExtracted = false,
                    Result = result,
                    ResultHashCode = resultHashCode,
                    Url = url
                };

                _dbContext.DownloadedResults.Add(downloadedResult);
                await _dbContext.SaveChangesAsync();


                ThreadPool.QueueUserWorkItem(new WaitCallback(async _ => {
                    try {
                        await _httpClient.PostAsJsonAsync("", downloadedResult);
                    } catch (Exception) {
                        
                    }
                }));

                return Ok();
            } catch (Exception) {

                return BadRequest(url);
            }
        }

        /// <summary>
        /// 下载指定的url集合
        /// </summary>
        /// <param name="urls">urls集合</param>
        /// <returns>不重复的HTML结果集</returns>
        public async Task<IHttpActionResult> Post(List<string> urls) {
            urls = urls.Distinct().ToList();

            var errorUrls = new ConcurrentBag<string>();
            var downloads = urls.Select(async url => {
                try {
                    var htmlPage = await _httpClient.GetStringAsync(url);
                    return new {
                        Url = url,
                        HtmlPage = htmlPage
                    };
                } catch (Exception) {
                    errorUrls.Add(url);
                    return null;
                }

            });

            var tasks = downloads.ToArray();
            var results = await Task.WhenAll(tasks);

            var downloadedResults = results
                .AsParallel()
                .Where(result => null != result)
                .Select(result => new DownloadedResult {
                    Domain = new Uri(result.Url).Host,
                    DownloadDate = _dateTime,
                    IsExtracted = false,
                    LastExtractDate = _dateTime,
                    Result = result.HtmlPage,
                    ResultHashCode = result.HtmlPage.GetMD5HashCode(),
                    Url = result.Url
                })
                .Except(_dbContext.DownloadedResults.AsParallel())
                .ToList();
            try {
                _dbContext.DownloadedResults.AddRange(downloadedResults);
                await _dbContext.SaveChangesAsync();
            } catch (Exception) {

                return BadRequest(JsonConvert.SerializeObject(urls));
            }


            return Ok(urls.Except(errorUrls));
        }
    }
}
