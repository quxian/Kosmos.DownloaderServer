using Kosmos.DownloaderServer.DbContext;
using Kosmos.DownloaderServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.DownloaderServer.Controllers {
    public class ValuesController : ApiController {
        private readonly AppDbContext _dbContext;

        public ValuesController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        // GET api/values
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public async Task<IHttpActionResult> Get(string id) {
            var test = new DownloadedResult {
                ResultHashCode = id,
                IsExtracted = false
            };

            _dbContext.DownloadedResults.Add(test);
            await _dbContext.SaveChangesAsync();

            return Ok(test);
        }

        // POST api/values
        public void Post([FromBody]string value) {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        public void Delete(int id) {
        }
    }
}
