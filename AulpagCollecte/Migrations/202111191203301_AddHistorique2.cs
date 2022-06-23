namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistorique2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Historiques", "date_api", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Historiques", "date_api");
        }
    }
}
