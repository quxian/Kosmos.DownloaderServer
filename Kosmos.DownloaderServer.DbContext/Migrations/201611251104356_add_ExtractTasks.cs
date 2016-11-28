namespace Kosmos.DownloaderServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_ExtractTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtractTasks",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        DownloadedResultHashCode = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => new { t.Name, t.DownloadedResultHashCode });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExtractTasks");
        }
    }
}
