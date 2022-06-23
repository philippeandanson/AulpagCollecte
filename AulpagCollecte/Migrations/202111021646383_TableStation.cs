namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableStation : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Stations", "tri", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Stations", "tri");
        }
    }
}
