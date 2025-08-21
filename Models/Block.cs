namespace WorkoutCardWebApp.Models;

public class Block
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Exercise> Exercises { get; set; } = new();
    public int? Sets { get; set; } // Number of times to repeat this block
    public int? RestTime { get; set; } // Rest time between block repetitions in seconds
    public string? Notes { get; set; }
}