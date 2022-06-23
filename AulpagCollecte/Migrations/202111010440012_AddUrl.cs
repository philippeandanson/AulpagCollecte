namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Stops");
            AlterColumn("public.Stops", "stop_id", c => c.String());
            AlterColumn("public.Stops", "station_id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Stops", "station_id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Stops");
            AlterColumn("public.Stops", "station_id", c => c.String());
            AlterColumn("public.Stops", "stop_id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Stops", "stop_id");
        }
    }
}
