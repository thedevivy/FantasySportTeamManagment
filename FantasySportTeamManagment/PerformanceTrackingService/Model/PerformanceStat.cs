using System.ComponentModel.DataAnnotations;

namespace PerformanceTrackingService.Model
{
    public class PerformanceStat
    {
        [Key]
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public string ?PlayerName { get; set; }

        public string ?Position { get; set; }

        public int TeamId { get; set; }

        public string ?TeamName { get; set; }

        public int Points { get; set; }

        public int Assists { get; set; }

        public int Fouls { get; set; }

    }
}
