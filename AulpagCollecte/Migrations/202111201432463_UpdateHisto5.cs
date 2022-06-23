namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Histo_trips");
            AlterColumn("public.Histo_trips", "mode", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Histo_trips", new[] { "trip_id", "mode" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Histo_trips");
            AlterColumn("public.Histo_trips", "mode", c => c.String());
            AddPrimaryKey("public.Histo_trips", "trip_id");
        }
    }
}
