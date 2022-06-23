namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Histo_trips", "mode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Histo_trips", "mode");
        }
    }
}
