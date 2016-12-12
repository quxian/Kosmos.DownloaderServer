using Kosmos.DownloaderServer.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Kosmos.DownloaderServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IocConfig.RegisterDependencies();

            using (var dbContext = new AppDbContext())
            {
                ResultCahce.CacheToDb(dbContext);
            }
        }

        void Application_End(object sender, EventArgs e)
        {

            using (var dbContext = new AppDbContext())
            {
                ResultCahce.CacheToDb(dbContext);
            }
        }
    }
}
