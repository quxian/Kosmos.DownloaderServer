namespace Kosmos.DownloaderServer.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_DownloadedResult_Depth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownloadedResults", "Depth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DownloadedResults", "Depth");
        }
    }
}
