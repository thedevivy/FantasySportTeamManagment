using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace FantasySportTeamManagment.Model
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

    }

   
}
