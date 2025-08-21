using System.ComponentModel.DataAnnotations;

namespace WorkoutCardWebApp.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Exercise name is required")]
        [StringLength(100, ErrorMessage = "Exercise name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public ExerciseType Type { get; set; }
        
        public string? TargetMuscleGroup { get; set; }
        
        public List<Set> Sets { get; set; } = new List<Set>();
        
        public string? Notes { get; set; }
    }
    
    public enum ExerciseType
    {
        Strength,
        Cardio,
        Flexibility,
        Balance,
        Other
    }
}