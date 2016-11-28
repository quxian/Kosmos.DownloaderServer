using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kosmos.DownloaderServer;
using Kosmos.DownloaderServer.Controllers;
using Kosmos.DownloaderServer.DbContext;

namespace Kosmos.DownloaderServer.Tests.Controllers {
    [TestClass]
    public class ValuesControllerTest {
        [TestMethod]
        public void Get() {
            // Arrange
            ValuesController controller = new ValuesController(new AppDbContext());

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

    }
}
