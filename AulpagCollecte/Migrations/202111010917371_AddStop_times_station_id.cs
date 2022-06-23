namespace AulpagCollecte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStop_times_station_id : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Stop_times", "station_id", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Stop_times", "station_id");
        }
    }
}
