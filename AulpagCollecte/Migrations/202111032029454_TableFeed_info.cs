namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableFeed_info : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Feed_info",
                c => new
                    {
                        feed_version = c.DateTime(nullable: false),
                        conv_rev = c.String(nullable: false, maxLength: 128),
                        feed_start_date = c.Int(nullable: false),
                        feed_end_date = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.feed_version, t.conv_rev });
            
        }
        
        public override void Down()
        {
            DropTable("public.Feed_info");
        }
    }
}
