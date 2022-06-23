namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStops : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Stops",
                c => new
                    {
                        stop_id = c.String(nullable: false, maxLength: 128),
                        stop_name = c.String(),
                        stop_desc = c.String(),
                        stop_lat = c.String(),
                        stop_lon = c.String(),
                    })
                .PrimaryKey(t => t.stop_id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Stops");
        }
    }
}
