namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCalendrierTypeInt : DbMigration
    {
        public override void Up()
        {
            DropTable("public.Calendriers");
        }
        
        public override void Down()
        {
            CreateTable(
                "public.Calendriers",
                c => new
                    {
                        date_calendrier = c.String(nullable: false, maxLength: 128),
                        jour_semaine = c.Int(nullable: false),
                        ferie = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.date_calendrier);
            
        }
    }
}
