namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCalendar_list : DbMigration
    {
        public override void Up()
        {
            DropTable("public.Calendar_list");
        }
        
        public override void Down()
        {
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
            
        }
    }
}
