using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Model;

namespace PerformanceTrackingService.Data
{
    public class PerformanceDbContext:  DbContext
    {
        // Constructor
        public PerformanceDbContext(DbContextOptions<PerformanceDbContext> options) : base(options)
        {
            //empty
        }

        public DbSet<PerformanceStat> PerformanceStats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerformanceStat>().HasData(
                new PerformanceStat { Id = 1, PlayerId = 101, TeamId = 1, Points = 0, Assists = 0, Fouls = 0 },
                new PerformanceStat { Id = 2, PlayerId = 102, TeamId = 1, Points = 0, Assists = 0, Fouls = 0 },
                new PerformanceStat { Id = 3, PlayerId = 103, TeamId = 2, Points = 0, Assists = 0, Fouls = 0 }
            );
        }
    }
    
}
