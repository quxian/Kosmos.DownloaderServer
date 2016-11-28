using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kosmos.DownloaderServer;
using Kosmos.DownloaderServer.Controllers;
using Kosmos.DownloaderServer.DbContext;

namespace Kosmos.DownloaderServer.Tests.Controllers {
    [TestClass]
    public class HomeControllerTest {
        [TestMethod]
        public void Index() {
            // Arrange
            HomeController controller = new HomeController(new AppDbContext());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
