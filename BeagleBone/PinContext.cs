using Microsoft.EntityFrameworkCore;
using static Sqlite.DBHelper;

namespace BeagleBone
{
    class PinContext : DbContext
    {
        public DbSet<PinRecord> PinRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=logbook.db;");
        }
    }

}
