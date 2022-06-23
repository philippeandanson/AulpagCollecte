namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistorique3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Historiques");
            AddPrimaryKey("public.Historiques", new[] { "trip_headsign", "service_id", "station_id", "departure", "date_api" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Historiques");
            AddPrimaryKey("public.Historiques", new[] { "trip_headsign", "service_id", "station_id", "departure" });
        }
    }
}
