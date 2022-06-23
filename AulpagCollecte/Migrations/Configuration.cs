using System.Data.Entity.Migrations;

namespace AulpagCollecte.Migrations
{
    

    internal sealed class Configuration : DbMigrationsConfiguration<AulpagCollecte.Data.BaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.BaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }


        //   "202110301020261_InitialCreate"; "AulpagCollecte.Data.BaseContext"; "<données binaires>"; "6.4.4"
        //   "202110301306358_AddStops"; "AulpagCollecte.Data.BaseContext"; "<données binaires>"; "6.4.4"
    }
}
