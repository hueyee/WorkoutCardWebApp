namespace WorkoutCardWebApp.Models;

public class Workout
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Block> Blocks { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    public string? Difficulty { get; set; }
    public List<string> Tags { get; set; } = new();
}