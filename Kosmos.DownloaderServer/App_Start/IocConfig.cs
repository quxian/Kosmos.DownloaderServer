using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Kosmos.DownloaderServer.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Kosmos.DownloaderServer {
    public class IocConfig {
        public static void RegisterDependencies() {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<AppDbContext>();
            builder.RegisterType<HttpClient>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}