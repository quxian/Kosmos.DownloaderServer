using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Kosmos.DownloaderServer.Model;
using Kosmos.DownloaderServer.ModelDbMappings;

namespace Kosmos.DownloaderServer.DbContext {
    public class AppDbContext : System.Data.Entity.DbContext {
        public AppDbContext() : base("DownloaderServerDbConnection") { }

        public DbSet<DownloadedResult> DownloadedResults { get; set; }
        public DbSet<ExtractTask> ExtractTasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new DownloaderResultMap());
            modelBuilder.Configurations.Add(new ExtractTaskMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
