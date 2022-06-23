namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHisto62 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.Histo_trips", "date_fin", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("public.Histo_trips", "date_fin", c => c.DateTime(nullable: false));
        }
    }
}
