using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlayerManagementService.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; } 
        public string Position { get; set; }
        public int TeamId { get; set; }
        public bool isDrafted { get; set; } = false;

    }
}
