namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajHeader : DbMigration
    {
        public override void Up()
        {
            DropTable("public.Headers");
        }
        
        public override void Down()
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
            
        }
    }
}
