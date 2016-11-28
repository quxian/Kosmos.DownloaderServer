using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.DownloadServer.TestsForConsoleApp {
    class Program {
        static void Main(string[] args) {
            TestDownloadControllerPost();
        }

        private static void TestDownloadControllerPost() {
            using (var httpClient = new HttpClient()) {
                var result = httpClient.PostAsJsonAsync("http://localhost:52810/Api/Download", new List<string> {
                    "https://www.baidu.com/",
                    "http://stackoverflow.com/questions/19158378/httpclient-not-supporting-postasjsonasync-method-c-sharp"
                }).Result;

                Console.WriteLine(result);
            }
        }
    }
}
