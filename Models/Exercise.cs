namespace WorkoutCardWebApp.Models;

public class Exercise
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public List<Set> Sets { get; set; } = new();
    public string? Notes { get; set; }
    public int? RestTime { get; set; } // in seconds
}