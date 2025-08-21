using System.Net.Http.Json;
using System.Text.Json;
using WorkoutCardWebApp.Shared.Models;
using WorkoutCardWebApp.Shared.Services;

namespace WorkoutCardWebApp.Client.Services;

public class ApiWorkoutStorageService : IWorkoutStorageService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiWorkoutStorageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<List<Workout>> GetWorkoutsAsync(string username)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/workouts/{username}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Workout>>(json, _jsonOptions) ?? new List<Workout>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workouts: {ex.Message}");
            return new List<Workout>();
        }
    }

    public async Task<Workout?> GetWorkoutAsync(string username, string workoutId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/workouts/{username}/{workoutId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Workout>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workout: {ex.Message}");
            return null;
        }
    }

    public async Task<Workout> CreateWorkoutAsync(string username, Workout workout)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/workouts/{username}", workout, _jsonOptions);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Workout>(json, _jsonOptions) ?? workout;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating workout: {ex.Message}");
            throw;
        }
    }

    public async Task<Workout?> UpdateWorkoutAsync(string username, string workoutId, Workout workout)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/workouts/{username}/{workoutId}", workout, _jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Workout>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating workout: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteWorkoutAsync(string username, string workoutId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/workouts/{username}/{workoutId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting workout: {ex.Message}");
            return false;
        }
    }
}