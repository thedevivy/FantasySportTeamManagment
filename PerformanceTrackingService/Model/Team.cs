using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;

namespace PerformanceTrackingService.Model
{
    public class Team
    {
        [Key]
        [JsonPropertyName("teamId")]
        public int TeamId { get; set; }

        [JsonPropertyName("teamName")]
        public string TeamName { get; set; }

        [JsonPropertyName("players")]
        public List<Player> Players { get; set; }
    }


}
