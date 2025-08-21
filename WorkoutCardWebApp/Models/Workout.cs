using System.ComponentModel.DataAnnotations;

namespace WorkoutCardWebApp.Models
{
    public class Workout
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Workout name is required")]
        [StringLength(100, ErrorMessage = "Workout name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastModifiedDate { get; set; }
        
        public List<Block> Blocks { get; set; } = new List<Block>();
        
        public WorkoutStatus Status { get; set; } = WorkoutStatus.Draft;
        
        public int? EstimatedDurationMinutes { get; set; }
        
        public string? Notes { get; set; }
        
        public List<string> Tags { get; set; } = new List<string>();
    }
    
    public enum WorkoutStatus
    {
        Draft,
        Active,
        Completed,
        Archived
    }
}