namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTripsHeaders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Headers",
                c => new
                    {
                        trip_headsign = c.Int(nullable: false, identity: true),
                        Order_line = c.String(),
                        Icon_header = c.String(),
                        Icon_service = c.String(),
                        sens = c.String(),
                    })
                .PrimaryKey(t => t.trip_headsign);
            
            AddColumn("public.Trips", "tri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Trips", "tri");
            DropTable("public.Headers");
        }
    }
}
