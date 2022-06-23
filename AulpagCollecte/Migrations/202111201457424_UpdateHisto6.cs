namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto6 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Histo_trips");
            AddColumn("public.Histo_trips", "date_fin", c => c.DateTime());
            AddPrimaryKey("public.Histo_trips", "trip_id");
            DropColumn("public.Histo_trips", "mode");
        }
        
        public override void Down()
        {
            AddColumn("public.Histo_trips", "mode", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("public.Histo_trips");
            DropColumn("public.Histo_trips", "date_fin");
            AddPrimaryKey("public.Histo_trips", new[] { "trip_id", "mode" });
        }
    }
}
