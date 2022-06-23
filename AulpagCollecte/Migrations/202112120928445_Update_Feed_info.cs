namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Feed_info : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Feed_info", "date_d", c => c.DateTime(nullable: false));
            AddColumn("public.Feed_info", "date_f", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Feed_info", "date_f");
            DropColumn("public.Feed_info", "date_d");
        }
    }
}
