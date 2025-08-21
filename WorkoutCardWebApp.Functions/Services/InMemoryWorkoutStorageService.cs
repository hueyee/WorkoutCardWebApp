using System.Collections.Concurrent;
using WorkoutCardWebApp.Shared.Models;
using WorkoutCardWebApp.Shared.Services;

namespace WorkoutCardWebApp.Functions.Services;

public class InMemoryWorkoutStorageService : IWorkoutStorageService
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Workout>> _storage = new();

    public Task<List<Workout>> GetWorkoutsAsync(string username)
    {
        if (_storage.TryGetValue(username, out var userWorkouts))
        {
            return Task.FromResult(userWorkouts.Values.OrderByDescending(w => w.CreatedAt).ToList());
        }
        
        return Task.FromResult(new List<Workout>());
    }

    public Task<Workout?> GetWorkoutAsync(string username, string workoutId)
    {
        if (_storage.TryGetValue(username, out var userWorkouts) &&
            userWorkouts.TryGetValue(workoutId, out var workout))
        {
            return Task.FromResult<Workout?>(workout);
        }
        
        return Task.FromResult<Workout?>(null);
    }

    public Task<Workout> CreateWorkoutAsync(string username, Workout workout)
    {
        workout.Id = Guid.NewGuid().ToString();
        workout.CreatedAt = DateTime.UtcNow;
        workout.UpdatedAt = DateTime.UtcNow;

        var userWorkouts = _storage.GetOrAdd(username, _ => new ConcurrentDictionary<string, Workout>());
        userWorkouts[workout.Id] = workout;

        return Task.FromResult(workout);
    }

    public Task<Workout?> UpdateWorkoutAsync(string username, string workoutId, Workout workout)
    {
        if (_storage.TryGetValue(username, out var userWorkouts) &&
            userWorkouts.TryGetValue(workoutId, out var existingWorkout))
        {
            workout.Id = workoutId;
            workout.CreatedAt = existingWorkout.CreatedAt;
            workout.UpdatedAt = DateTime.UtcNow;
            
            userWorkouts[workoutId] = workout;
            return Task.FromResult<Workout?>(workout);
        }
        
        return Task.FromResult<Workout?>(null);
    }

    public Task<bool> DeleteWorkoutAsync(string username, string workoutId)
    {
        if (_storage.TryGetValue(username, out var userWorkouts))
        {
            return Task.FromResult(userWorkouts.TryRemove(workoutId, out _));
        }
        
        return Task.FromResult(false);
    }
}