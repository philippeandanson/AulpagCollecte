namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("public.Stop_times", "arrival_time");
            DropColumn("public.Stop_times", "departure_time");
        }
        
        public override void Down()
        {
            AddColumn("public.Stop_times", "departure_time", c => c.DateTime(nullable: false));
            AddColumn("public.Stop_times", "arrival_time", c => c.DateTime(nullable: false));
        }
    }
}
