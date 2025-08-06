using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Model;
using System.Text.Json;

namespace PerformanceTrackingService.Controller
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
        public async Task <IActionResult> SimulateCompetition()
        {
            var stats = await context.PerformanceStats.Include(p=>p.TeamId).Include(p=>p.PlayerId).ToListAsync();
            var random = new Random();

            foreach (var stat in stats)
            {
                //randomly generate performance stats
                stat.Points += random.Next(0, 20);  
                stat.Assists += random.Next(0, 10);  
                stat.Fouls += random.Next(0, 3);

                await updateTeamAndPlayerStats(stat);
            }

            await context.SaveChangesAsync();
            return Ok(stats);

        }

        // GET api/Competitions to get all performance stats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformanceStat>>> Get()
        {
            return Ok(await context.PerformanceStats.ToListAsync());
        }

        // put api/Competitions/1 to update performance stat by id
        [HttpPut("{id}")]
        public async Task <IActionResult> Put(int id, [FromBody] PerformanceStat updatedStat)
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

            await context.SaveChangesAsync();//save changes to the database

            return NoContent();
        }
        
        private async Task updateTeamAndPlayerStats(PerformanceStat stat)
        {
            //team and player url
            string teamUrl = $"https://localhost:7217/api/Teams/{stat.TeamId}";
            string playerUrl = $"https://localhost:5001/api/Players/{stat.PlayerId}";

            try
            {
                var teamResponse = await client.GetAsync(teamUrl);
                if (teamResponse.IsSuccessStatusCode)
                {
                        var teamContent = await teamResponse.Content.ReadAsStringAsync();
                        var team = JsonSerializer.Deserialize<Team>(teamContent);
                        stat.TeamId = (int)(team?.Id);
                        stat.TeamName = team?.Name;
                }

                var playerResponse = await client.GetAsync(playerUrl);
                if (playerResponse.IsSuccessStatusCode)
                {
                    var playerContent = await playerResponse.Content.ReadAsStringAsync();
                    var player = JsonSerializer.Deserialize<Player>(playerContent);
                    stat.PlayerId = (int)(player?.Id);
                    stat.PlayerName = player?.Name;
                    stat.Position = player?.Position;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }
        }
    }
}
