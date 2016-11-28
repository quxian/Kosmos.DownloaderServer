using Kosmos.DownloaderServer.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kosmos.DownloaderServer.Controllers {
    public class HomeController : Controller {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }


        public ActionResult Index() {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
