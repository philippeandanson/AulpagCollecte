namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistorique : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Historiques",
                c => new
                    {
                        trip_headsign = c.Int(nullable: false),
                        service_id = c.Int(nullable: false),
                        station_id = c.Int(nullable: false),
                        departure = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.trip_headsign, t.service_id, t.station_id, t.departure });
            
        }
        
        public override void Down()
        {
            DropTable("public.Historiques");
        }
    }
}
