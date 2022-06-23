using AulpagCollecte.Models;
using System.Data.Entity;

namespace AulpagCollecte.Data
{
    class BaseContext : DbContext
    {     
        public DbSet<Stop_times> stop_times { get; set; }
        public DbSet<Trips> trips { get; set; }
        public DbSet<Calendar_dates> calendar_dates { get; set; }
        public DbSet<Calendar_list> calendar_list { get; set; }
        public DbSet<Stops> stops { get; set; }
        public DbSet<Stations> stations { get; set; }
        public DbSet<Tads> tads { get; set; }
        public DbSet<Feed_info> feed_info { get; set; }
        public DbSet<Calendriers> calendrier { get; set; }
        public DbSet<Headers> headers { get; set; }
        public DbSet<Histo_trips> histo_trips { get; set; }
        public DbSet<Histo_stop_times> histo_stop_times { get; set; }

        public BaseContext() : base(nameOrConnectionString: "Default") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    } 
}
