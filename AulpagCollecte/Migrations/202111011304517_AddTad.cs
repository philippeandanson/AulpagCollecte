namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Tads",
                c => new
                    {
                        id_station = c.String(nullable: false, maxLength: 128),
                        stop_headsign = c.String(nullable: false, maxLength: 128),
                        arrival_time = c.String(),
                        departure_time = c.String(),
                    })
                .PrimaryKey(t => new { t.id_station, t.stop_headsign });
            
        }
        
        public override void Down()
        {
            DropTable("public.Tads");
        }
    }
}
