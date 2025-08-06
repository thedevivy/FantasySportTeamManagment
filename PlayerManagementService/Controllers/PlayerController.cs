using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;
using PlayerManagementService.Model;

namespace PlayerManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerContext context;
        
        public PlayerController(PlayerContext context)
        {
            this.context = context;
        }

        // GET /api/player/getLoasters?teamId=1
        [HttpGet("getTeamRoaster")]
        public IActionResult GetTeamRoaster([FromQuery] int teamId)
        {
            if (teamId < 0)
                return BadRequest("Invalid ID");


            var teamPlayers = context.Player.Where(p => p.TeamId == teamId).ToList();

            if (teamPlayers.Count == 0)
                return NotFound(new { Message = "No players found for the given team" });


            return Ok(teamPlayers);
        }


        // draft status handle
        // POST: api/PlayerManagement/draft
        [HttpPost("draft")]
        public async Task<ActionResult> DraftPlayer([FromBody] DraftPlayerRequest request)
        {
            var player = await context.Player.FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);
            if (player == null)
            {
                return NotFound(new { Message = $"Player with ID {request.PlayerId} not found." });
            }

            if (player.isDrafted)
            {
                return BadRequest(new { Message = $"Player {player.Name} is already drafted." });
            }

            player.isDrafted = true;
            context.Player.Update(player);

            await context.SaveChangesAsync();

            
            return Ok(new { Message = $"Player {player.Name} successfully drafted to Team {request.TeamId}." });
        }


        // release draft handle
        // POST: api/PlayerManagement/draft
        [HttpPost("release")]
        public async Task<ActionResult> ReleasePlayer([FromBody] DraftPlayerRequest request)
        {
            // find player
            var player = await context.Player.FirstOrDefaultAsync(p => p.PlayerId == request.PlayerId);
            if (player == null)
            {
                return NotFound(new { Message = $"Player with ID {request.PlayerId} not found." });
            }
            // check status
            if (!player.isDrafted)
            {
                return BadRequest(new { Message = $"Player {player.Name} is not drafted to any team." });
            }

            player.isDrafted = false;
            context.Player.Update(player);

            await context.SaveChangesAsync();


            return Ok(new { Message = $"Player {player.Name} successfully released from Team {request.TeamId}." });
        }

    }
}
