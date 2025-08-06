using FantasySportTeamManagment.Model;
using Microsoft.EntityFrameworkCore;

namespace FantasySportTeamManagment.Data
{
    public class TeamContext :DbContext
    {

        //contructor
        public TeamContext(DbContextOptions<TeamContext> options) : base(options)
        {
            // empty
        }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasData(
                new Team { Id = 1, TeamName = "team1", CreatedAt = DateTime.Now },
                 new Team { Id = 2, TeamName = "team2", CreatedAt = DateTime.Now },
                 new Team { Id = 3, TeamName = "team3", CreatedAt = DateTime.Now }


                );
        }
    }
}
