namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnnulCalendrier : DbMigration
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
                        date_calendrier = c.Int(nullable: false, identity: true),
                        jour_semaine = c.Int(nullable: false),
                        ferie = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.date_calendrier);
            
        }
    }
}
