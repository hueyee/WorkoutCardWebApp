namespace WorkoutCardWebApp.Shared.Models;

public class Set
{
    public int? Reps { get; set; }
    public double? Weight { get; set; }
    public TimeSpan? Duration { get; set; }
    public double? Distance { get; set; }
    public string? Notes { get; set; }
    public bool Completed { get; set; } = false;
}