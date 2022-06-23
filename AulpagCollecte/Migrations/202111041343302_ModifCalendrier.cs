namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCalendrier : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Calendriers");
            AlterColumn("public.Calendriers", "date_calendrier", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Calendriers", "date_calendrier");
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Calendriers");
            AlterColumn("public.Calendriers", "date_calendrier", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("public.Calendriers", "date_calendrier");
        }
    }
}
