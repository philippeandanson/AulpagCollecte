namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTad4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Tads");
            AlterColumn("public.Tads", "stop_headsign", c => c.Int(nullable: false));
            AddPrimaryKey("public.Tads", new[] { "id_station", "stop_headsign" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Tads");
            AlterColumn("public.Tads", "stop_headsign", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Tads", new[] { "id_station", "stop_headsign" });
        }
    }
}
