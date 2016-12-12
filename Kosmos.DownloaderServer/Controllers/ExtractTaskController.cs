using Kosmos.DownloaderServer.DbContext;
using Kosmos.DownloaderServer.Model;
using Kosmos.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.DownloaderServer.Controllers {
    public class ExtractTaskController : ApiController {
        private readonly AppDbContext _dbContext;

        public ExtractTaskController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }


        /// <summary>
        /// 创建需要提取的下载结果的任务
        /// </summary>
        /// <param name="name">任务名</param>
        /// <param name="domain">提取限制</param>
        /// <returns>任务是否创建成功，基于http状态码</returns>
        [HttpGet]
        public async Task<IHttpActionResult> Create(string name, string domain) {
            if (null == name) {
                return BadRequest($"{nameof(ExtractTask)} Name 不能为空！");
            }
            if (null != _dbContext.ExtractTasks.FirstOrDefault(extractTask => extractTask.Name == name)) {
                return BadRequest($"{nameof(ExtractTask)} Name 已经存在！");
            }
            var downloadedResults = _dbContext.DownloadedResults.AsParallel();
            if (null != domain) {
                downloadedResults = downloadedResults.Where(downloadResult => null != downloadResult.Domain && downloadResult.Domain.Contains(domain));
            }

            _dbContext.ExtractTasks.AddRange(downloadedResults.Select(downloadedResult => new ExtractTask {
                DownloadedResultHashCode = downloadedResult.ResultHashCode,
                Name = name
            }));

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {

                return BadRequest($"创建 {nameof(ExtractTask)} 任务失败，请重试！{e.Message}");
            }

            return Ok(name);
        }

        /// <summary>
        /// 提取任务
        /// </summary>
        /// <param name="name">任务名</param>
        /// <returns>下载结果</returns>
        [HttpGet]
        public async Task<IHttpActionResult> Extract(string name) {
            try {
                if (null == _dbContext.ExtractTasks.AsParallel().FirstOrDefault(extractTask => extractTask.Name == name))
                    return Ok($"任务已完成：{name}！");
            } catch (Exception e) {
                SingleHttpClient.PostException(e);

                return BadRequest(e.Message);
            }

            try {
                var firstTaskItem = _dbContext.ExtractTasks.First();
                _dbContext.ExtractTasks.Remove(firstTaskItem);
                await _dbContext.SaveChangesAsync();
                var downloadedResult = await _dbContext.DownloadedResults.FindAsync(firstTaskItem.DownloadedResultHashCode);

                return Ok(downloadedResult);
            } catch (Exception e) {
                SingleHttpClient.PostException(e);

                return BadRequest(e.Message);
            }
        }
    }
}
