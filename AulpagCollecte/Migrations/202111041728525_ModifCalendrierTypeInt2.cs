namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCalendrierTypeInt2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Calendriers",
                c => new
                    {
                        date_calendrier = c.Int(nullable: false, identity: true),
                        jour_semaine = c.Int(nullable: false),
                        ferie = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.date_calendrier);
            
        }
        
        public override void Down()
        {
            DropTable("public.Calendriers");
        }
    }
}
