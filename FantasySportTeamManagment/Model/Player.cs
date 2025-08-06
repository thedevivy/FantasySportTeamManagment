using System.ComponentModel.DataAnnotations;

namespace FantasySportTeamManagment.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public bool isDrafted { get; set; }

    }

}
