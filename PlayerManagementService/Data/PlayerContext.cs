using PlayerManagementService.Model;
using Microsoft.EntityFrameworkCore;

namespace PlayerManagementService.Data
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
        {
            //empty
        }

        public DbSet<Player> Player { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    PlayerId = 1,               
                    Name = "John Go",
                    Position = "Front Guard",
                    TeamId = 1,
                    isDrafted = false
                },
                new Player
                {
                    PlayerId = 2,
                    Name = "Alice Swift",
                    Position = "Center Forward",
                    TeamId = 1,
                    isDrafted = false
                },
                new Player
                {
                    PlayerId = 3,
                    Name = "Bob Lee",
                    Position = "Point Guard",
                    TeamId = 2,
                    isDrafted = false
                },
                new Player
                {
                    PlayerId = 4,
                    Name = "Tom Jones",
                    Position = "Point Guard",
                    TeamId = 2,
                    isDrafted = false
                },
                new Player
                {
                    PlayerId = 5,
                    Name = "Samantha Green",
                    Position = "Shooting Guard",
                    TeamId = 2,
                    isDrafted = false
                });
        }


    }
}
