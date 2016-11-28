using Kosmos.DownloaderServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Kosmos.DownloaderServer.ModelDbMappings {
    public class DownloaderResultMap : EntityTypeConfiguration<DownloadedResult> {
        public DownloaderResultMap() {
            this.HasKey(downloaderResult => downloaderResult.ResultHashCode);
            this.Property(downloaderResult => downloaderResult.ResultHashCode)
                .HasMaxLength(32);
        }
    }
}
