namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCalendar_list : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Calendar_list", "week_d", c => c.Int(nullable: false));
            AddColumn("public.Calendar_list", "week_f", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Calendar_list", "week_f");
            DropColumn("public.Calendar_list", "week_d");
        }
    }
}
