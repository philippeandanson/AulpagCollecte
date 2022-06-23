namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifHeader : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("public.Headers");
            AlterColumn("public.Headers", "trip_headsign", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("public.Headers", "trip_headsign");
        }
        
        public override void Down()
        {
            DropPrimaryKey("public.Headers");
            AlterColumn("public.Headers", "trip_headsign", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("public.Headers", "trip_headsign");
        }
    }
}
