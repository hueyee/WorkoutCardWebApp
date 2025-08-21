using WorkoutCardWebApp.Models;

namespace WorkoutCardWebApp.Services;

public interface IWorkoutStorageService
{
    Task<List<Workout>> GetWorkoutsAsync(string username);
    Task<Workout?> GetWorkoutAsync(string username, string workoutId);
    Task<Workout> CreateWorkoutAsync(string username, Workout workout);
    Task<Workout?> UpdateWorkoutAsync(string username, string workoutId, Workout workout);
    Task<bool> DeleteWorkoutAsync(string username, string workoutId);
}