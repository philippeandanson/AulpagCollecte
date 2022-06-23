namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Stop_times", "arrival", c => c.String());
            AddColumn("public.Stop_times", "departure", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Stop_times", "departure");
            DropColumn("public.Stop_times", "arrival");
        }
    }
}
