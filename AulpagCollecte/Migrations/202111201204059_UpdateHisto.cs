namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Histo_stop_times",
                c => new
                    {
                        trip_id = c.String(nullable: false, maxLength: 128),
                        station_id = c.Int(nullable: false),
                        departure = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.trip_id, t.station_id, t.departure });
            
            CreateTable(
                "public.Histo_trips",
                c => new
                    {
                        trip_id = c.String(nullable: false, maxLength: 128),
                        departure = c.String(),
                        trip_headsign = c.String(),
                        passage = c.String(),
                        exempt = c.String(),
                        remarques = c.String(),
                    })
                .PrimaryKey(t => t.trip_id);
            
            DropTable("public.Historiques");
        }
        
        public override void Down()
        {
            CreateTable(
                "public.Historiques",
                c => new
                    {
                        trip_headsign = c.Int(nullable: false),
                        service_id = c.Int(nullable: false),
                        station_id = c.Int(nullable: false),
                        departure = c.String(nullable: false, maxLength: 128),
                        date_api = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.trip_headsign, t.service_id, t.station_id, t.departure, t.date_api });
            
            DropTable("public.Histo_trips");
            DropTable("public.Histo_stop_times");
        }
    }
}
