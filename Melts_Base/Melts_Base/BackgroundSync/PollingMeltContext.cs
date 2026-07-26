using Melts_Base.SQLiteModels;
using Microsoft.EntityFrameworkCore;

namespace Melts_Base.BackgroundSync
{
    internal sealed class PollingMeltContext : DbContext
    {
        private readonly string _databasePath;

        public PollingMeltContext(string databasePath) => _databasePath = databasePath;
        public DbSet<Melt> Melts => Set<Melt>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
    }
}
