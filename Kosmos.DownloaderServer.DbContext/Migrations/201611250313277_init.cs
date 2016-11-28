namespace Kosmos.DownloaderServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DownloadedResults",
                c => new
                    {
                        ResultHashCode = c.String(nullable: false, maxLength: 32),
                        Domain = c.String(),
                        Url = c.String(),
                        Result = c.String(),
                        DownloadDate = c.DateTime(),
                        IsExtracted = c.Boolean(nullable: false),
                        LastExtractDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ResultHashCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DownloadedResults");
        }
    }
}
