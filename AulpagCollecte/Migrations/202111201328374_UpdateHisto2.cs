namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Histo_trips", "date_api", c => c.DateTime(nullable: false));
            DropColumn("public.Histo_trips", "departure");
        }
        
        public override void Down()
        {
            AddColumn("public.Histo_trips", "departure", c => c.String());
            DropColumn("public.Histo_trips", "date_api");
        }
    }
}
