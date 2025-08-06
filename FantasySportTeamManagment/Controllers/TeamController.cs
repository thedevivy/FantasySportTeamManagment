using FantasySportTeamManagment.Data;
using FantasySportTeamManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FantasySportTeamManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly TeamContext context;
        private readonly HttpClient _httpClient;

        public TeamController(HttpClient httpClient, TeamContext context)
        {
            this._httpClient = httpClient;
            this.context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTeam(string teamName)
        {



            if (string.IsNullOrEmpty(teamName))
                return BadRequest("Team name cannot be empty.");

            // Ensure the team's name is unique
            var existingTeam = await context.Teams
                .FirstOrDefaultAsync(t => t.TeamName.ToLower() == teamName.ToLower());

            if (existingTeam != null)
            {
                return BadRequest("The team name is already used.");
            }

            var team = new Team
            {
                TeamName = teamName,
                CreatedAt = DateTime.Now
            };

            try
            {
                context.Teams.Add(team);
                await context.SaveChangesAsync(); // Save changes and persist the new team in DB

                return Ok(new { Message = $"Team '{teamName}' created successfully.", TeamId = team.Id });
            }
            catch (Exception ex)
            {
                // Handle any database errors
                return StatusCode(500, new { Message = "Error creating the team.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> Get()
        {
            var teams = await context.Teams.ToListAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> Get(int teamId)
        {
            var team = await context.Teams.FirstOrDefaultAsync(p => p.Id == teamId);

            if (team == null)
            {
                return NotFound(new { Message = $"Team with ID {teamId} not found." });
            }

            return Ok(team);
        }

        [HttpGet("{teamId}/roster")]
        public async Task<IActionResult> GetTeamRoster(int teamId)
        {
            // Validate teamId
            if (teamId <= 0)
            {
                return BadRequest(new { Message = "Invalid Team ID" });
            }

            var team = await context.Teams.FirstOrDefaultAsync(p => p.Id == teamId);
            if (team == null)
            {
                return NotFound(new { Message = $"Team with ID {teamId} does not exist." });
            }

            // Prepare the URL for the Player Management Service
            var playerServiceUrl = $"https://player-management-service.azurewebsites.net/api/player/getTeamRoaster?teamId={teamId}";

            try
            {
                // Make an HTTP GET request to the Player Management Service
                var response = await _httpClient.GetAsync(playerServiceUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the JSON response from the Player Management Service
                    var playerData = JsonConvert.DeserializeObject<List<Player>>(await response.Content.ReadAsStringAsync());

                    // Check if there are players assigned to the team
                    if (playerData == null || !playerData.Any())
                    {
                        return Ok(new
                        {
                            TeamId = teamId,
                            TeamName = team.TeamName,
                            Message = "This team currently has no players."
                        });
                    }

                    // Construct the response
                    var result = new
                    {
                        TeamId = teamId,
                        TeamName = team.TeamName,
                        Players = playerData
                    };

                    return Ok(result);
                }
                else
                {
                    // Handle cases where the Player Management Service returns an error
                    return StatusCode((int)response.StatusCode, new { Message = "Failed to fetch players from Player Management Service" });
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors (e.g., Player Management Service is down)
                return StatusCode(500, new { Message = "Error communicating with Player Management Service", Error = ex.Message });
            }
        }

        [HttpPut("update/{teamId}")]
        public async Task<IActionResult> UpdateTeam(int teamId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest("Invalid team ID or team name.");

            var existingTeam = await context.Teams
                 .FirstOrDefaultAsync(t => t.TeamName.ToLower() == newName.ToLower());

            if (existingTeam != null)
            {
                return BadRequest("The team name is already used.");
            }

            var team = await context.Teams.FirstOrDefaultAsync(p => p.Id == teamId);
            if (team == null)
                return NotFound($"Team with ID {teamId} not found.");

            team.TeamName = newName;
            await context.SaveChangesAsync();

            return Ok(new { id = team.Id, teamName = team.TeamName });
        }
    }
}