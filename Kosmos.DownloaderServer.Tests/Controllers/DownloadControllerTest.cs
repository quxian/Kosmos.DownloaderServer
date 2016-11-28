using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kosmos.DownloaderServer.DbContext;
using System.Net.Http;
using Kosmos.DownloaderServer.Controllers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kosmos.DownloaderServer.Tests.Controllers {
    [TestClass]
    public class DownloadControllerTest {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private readonly HttpClient _httpClient = new HttpClient();

        [TestMethod]
        public void Get() {
            var downloadController = new DownloadController(_dbContext, _httpClient);
            var result = downloadController.Get("http://www.cnblogs.com/").Result;
        }

        [TestMethod]
        public void Post() {
            using (var dbContext = new AppDbContext())
            using (var httpClient = new HttpClient()) {
                var downloadController = new DownloadController(dbContext, httpClient);
                var urls = new List<string> {
                    "http://www.cnblogs.com/",
                    "http://mail.163.com/"
                };

                var u = JsonConvert.SerializeObject(urls);

                var result = downloadController.Post(urls).Result;
            }

        }
    }
}
