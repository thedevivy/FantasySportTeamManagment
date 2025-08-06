using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PerformanceTrackingService.Model
{
    public class Player
    {
        [Key]
        [JsonPropertyName("playerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("teamId")]
        public int TeamId { get; set; }
    }

}
