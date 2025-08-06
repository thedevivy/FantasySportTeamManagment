using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PerformanceTrackingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly PerformanceDbContext context;
        private readonly HttpClient client;

        public CompetitionsController(PerformanceDbContext context, IHttpClientFactory clientFactory)
        {
            this.context = context;
            this.client = clientFactory.CreateClient();
        }

        // Post api/Competitions/simulate
        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateCompetition()
        {
            try
            {
                var stats = await context.PerformanceStats.ToListAsync();
                var random = new Random();

                foreach (var stat in stats)
                {
                    stat.Points += random.Next(0, 20);
                    stat.Assists += random.Next(0, 10);
                    stat.Fouls += random.Next(0, 3);
                    await updateTeamAndPlayerStats(stat); // Update team and player info
                }

                await context.SaveChangesAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SimulateCompetition: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while simulating competition." });
            }
        }

        // GET api/Competitions to get all performance stats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformanceStat>>> Get()
        {
            try
            {
                var stats = await context.PerformanceStats.ToListAsync();
                foreach (var stat in stats)
                {
                    await updateTeamAndPlayerStats(stat); // Update each stat with team and player info
                }
                return Ok(stats);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get: {ex.Message}");
                return StatusCode(500, "Error retrieving performance statistics.");
            }
        }

        // PUT api/Competitions/1 to update performance stat by id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PerformanceStat updatedStat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stat = await context.PerformanceStats.FindAsync(id);
                if (stat == null)
                {
                    return NotFound();
                }

                stat.Points = updatedStat.Points;
                stat.Assists = updatedStat.Assists;
                stat.Fouls = updatedStat.Fouls;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Put: {ex.Message}");
                return StatusCode(500, "Error updating performance statistics.");
            }
        }

        private async Task updateTeamAndPlayerStats(PerformanceStat stat)
        {
            try
            {
                // Fetch team data
                string teamUrl = $"https://localhost:7217/api/Team/{stat.TeamId}/roster";
                var teamResponse = await client.GetAsync(teamUrl);
                if (teamResponse.IsSuccessStatusCode)
                {
                    var teamContent = await teamResponse.Content.ReadAsStringAsync();
                    var team = JsonSerializer.Deserialize<Team>(teamContent);

                    if (team != null)
                    {
                        Console.WriteLine($"Fetched Team: TeamId: {team.TeamId}, TeamName: {team.TeamName}");
                        stat.TeamName = team.TeamName;
                    }
                    else
                    {
                        Console.WriteLine($"Null team data returned for TeamId: {stat.TeamId}");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to fetch team data for TeamId: {stat.TeamId}, StatusCode: {teamResponse.StatusCode}");
                }

                // Fetch player data
                string playerUrl = $"https://localhost:7246/api/player/getTeamRoaster?teamId={stat.TeamId}";
                var playerResponse = await client.GetAsync(playerUrl);
                if (playerResponse.IsSuccessStatusCode)
                {
                    var playerContent = await playerResponse.Content.ReadAsStringAsync();
                    var players = JsonSerializer.Deserialize<List<Player>>(playerContent);

                    var player = players?.FirstOrDefault(p => p.PlayerId == stat.PlayerId);
                    if (player != null)
                    {
                        stat.PlayerName = player.Name;
                        stat.Position = player.Position;
                        Console.WriteLine($"Player Updated: {player.Name}, Position: {player.Position}, PlayerId: {player.PlayerId}");
                    }
                    else
                    {
                        Console.WriteLine($"No matching player found for PlayerId: {stat.PlayerId} in TeamId: {stat.TeamId}");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to fetch player data for TeamId: {stat.TeamId}, StatusCode: {playerResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in updateTeamAndPlayerStats: {ex.Message}");
            }
        }

    }
}
