namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifStops_pk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Stops");
            AlterColumn("public.Stops", "stop_id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Stops", new[] { "stop_id", "station_id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Stops");
            AlterColumn("public.Stops", "stop_id", c => c.String());
            AddPrimaryKey("public.Stops", "station_id");
        }
    }
}
