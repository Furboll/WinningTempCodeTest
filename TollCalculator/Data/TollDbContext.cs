using Microsoft.EntityFrameworkCore;
using TollCalculator.Models.EF;

namespace TollCalculator.Data
{
    public class TollDbContext : DbContext
    {
        public TollDbContext(DbContextOptions<TollDbContext> options) : base(options) { }
        public DbSet<TollFreeDate> TollFreeDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TollFreeDate>()
                .HasKey(t => t.Id);
        }
    }
}
