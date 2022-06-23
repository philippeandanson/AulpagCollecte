namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Calendar_dates",
                c => new
                    {
                        service_id = c.Int(nullable: false),
                        date = c.Int(nullable: false),
                        exception_type = c.Int(nullable: false),
                        rang = c.Int(nullable: false),
                        jour_semaine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.service_id, t.date });
            
            CreateTable(
                "public.Calendar_list",
                c => new
                    {
                        service_id = c.Int(nullable: false, identity: true),
                        du = c.DateTime(nullable: false),
                        au = c.DateTime(nullable: false),
                        passage = c.String(),
                        exempt = c.String(),
                        remarques = c.String(),
                        test = c.String(),
                    })
                .PrimaryKey(t => t.service_id);
            
            CreateTable(
                "public.Stations",
                c => new
                    {
                        station_id = c.String(nullable: false, maxLength: 128),
                        station_name = c.String(),
                    })
                .PrimaryKey(t => t.station_id);
            
            CreateTable(
                "public.Stop_times",
                c => new
                    {
                        trip_id = c.String(nullable: false, maxLength: 128),
                        stop_id = c.String(nullable: false, maxLength: 128),
                        arrival_time = c.DateTime(nullable: false),
                        departure_time = c.DateTime(nullable: false),
                        stop_sequence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.trip_id, t.stop_id });
            
            CreateTable(
                "public.Stops",
                c => new
                    {
                        stop_id = c.String(nullable: false, maxLength: 128),
                        stop_name = c.String(),
                        stop_lat = c.String(),
                        stop_lon = c.String(),
                        station_id = c.String(),
                    })
                .PrimaryKey(t => t.stop_id);
            
            CreateTable(
                "public.Trips",
                c => new
                    {
                        trip_id = c.String(nullable: false, maxLength: 128),
                        route_id = c.String(),
                        service_id = c.Int(nullable: false),
                        trip_headsign = c.Int(nullable: false),
                        direction_id = c.String(),
                        block_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.trip_id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Trips");
            DropTable("public.Stops");
            DropTable("public.Stop_times");
            DropTable("public.Stations");
            DropTable("public.Calendar_list");
            DropTable("public.Calendar_dates");
        }
    }
}
