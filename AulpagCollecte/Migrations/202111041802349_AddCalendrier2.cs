namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendrier2 : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("public.Calendriers");
        }
    }
}
