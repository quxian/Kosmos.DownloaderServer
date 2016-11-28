using Kosmos.DownloaderServer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.DownloaderServer.ModelDbMappings {
    public class ExtractTaskMap : EntityTypeConfiguration<ExtractTask> {
        public ExtractTaskMap() {
            this.HasKey(extractTask => new {
                extractTask.Name,
                extractTask.DownloadedResultHashCode
            });

            this.Property(extractTask => extractTask.DownloadedResultHashCode)
                 .HasMaxLength(32);
            this.Property(extractTask => extractTask.Name)
                .HasMaxLength(128);
        }
    }
}
