using System.Text.Json;
using WorkoutCardWebApp.Models;

namespace WorkoutCardWebApp.Services;

public class DiskBasedWorkoutStorageService : IWorkoutStorageService
{
    private readonly string _baseDirectory;
    private readonly JsonSerializerOptions _jsonOptions;

    public DiskBasedWorkoutStorageService()
    {
        _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Workouts");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<Workout>> GetWorkoutsAsync(string username)
    {
        var userDirectory = Path.Combine(_baseDirectory, username);
        if (!Directory.Exists(userDirectory))
        {
            return new List<Workout>();
        }

        var workouts = new List<Workout>();
        var jsonFiles = Directory.GetFiles(userDirectory, "*.json");

        foreach (var file in jsonFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var workout = JsonSerializer.Deserialize<Workout>(json, _jsonOptions);
                if (workout != null)
                {
                    workouts.Add(workout);
                }
            }
            catch (Exception ex)
            {
                // Log error but continue processing other files
                Console.WriteLine($"Error reading workout file {file}: {ex.Message}");
            }
        }

        return workouts.OrderByDescending(w => w.CreatedAt).ToList();
    }

    public async Task<Workout?> GetWorkoutAsync(string username, string workoutId)
    {
        var filePath = GetWorkoutFilePath(username, workoutId);
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<Workout>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading workout file {filePath}: {ex.Message}");
            return null;
        }
    }

    public async Task<Workout> CreateWorkoutAsync(string username, Workout workout)
    {
        workout.Id = Guid.NewGuid().ToString();
        workout.CreatedAt = DateTime.UtcNow;
        workout.UpdatedAt = DateTime.UtcNow;

        var userDirectory = Path.Combine(_baseDirectory, username);
        Directory.CreateDirectory(userDirectory);

        var filePath = GetWorkoutFilePath(username, workout.Id);
        var json = JsonSerializer.Serialize(workout, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);

        return workout;
    }

    public async Task<Workout?> UpdateWorkoutAsync(string username, string workoutId, Workout workout)
    {
        var filePath = GetWorkoutFilePath(username, workoutId);
        if (!File.Exists(filePath))
        {
            return null;
        }

        workout.Id = workoutId;
        workout.UpdatedAt = DateTime.UtcNow;

        var json = JsonSerializer.Serialize(workout, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);

        return workout;
    }

    public Task<bool> DeleteWorkoutAsync(string username, string workoutId)
    {
        var filePath = GetWorkoutFilePath(username, workoutId);
        if (!File.Exists(filePath))
        {
            return Task.FromResult(false);
        }

        try
        {
            File.Delete(filePath);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting workout file {filePath}: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    private string GetWorkoutFilePath(string username, string workoutId)
    {
        return Path.Combine(_baseDirectory, username, $"{workoutId}.json");
    }
}