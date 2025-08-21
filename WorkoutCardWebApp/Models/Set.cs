using System.ComponentModel.DataAnnotations;

namespace WorkoutCardWebApp.Models
{
    public class Set
    {
        public int Id { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Reps must be non-negative")]
        public int Reps { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be non-negative")]
        public double Weight { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Duration must be non-negative")]
        public int DurationSeconds { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Distance must be non-negative")]
        public double Distance { get; set; }
        
        public string? Notes { get; set; }
        
        public bool IsCompleted { get; set; }
    }
}