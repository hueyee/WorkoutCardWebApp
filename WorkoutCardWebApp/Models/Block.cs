using System.ComponentModel.DataAnnotations;

namespace WorkoutCardWebApp.Models
{
    public class Block
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Block name is required")]
        [StringLength(100, ErrorMessage = "Block name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public int Order { get; set; }
        
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        
        [Range(0, int.MaxValue, ErrorMessage = "Rest time must be non-negative")]
        public int RestTimeSeconds { get; set; }
        
        public string? Notes { get; set; }
    }
}