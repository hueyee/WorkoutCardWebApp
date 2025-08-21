using System.Text.Json;
using WorkoutCardWebApp.Models;

namespace WorkoutCardWebApp.Services
{
    public class WorkoutStorageService : IWorkoutStorageService
    {
        private readonly string _baseDirectory;
        private readonly JsonSerializerOptions _jsonOptions;

        public WorkoutStorageService(IWebHostEnvironment environment)
        {
            _baseDirectory = Path.Combine(environment.ContentRootPath, "Workouts");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<Workout>> GetWorkoutsAsync(string username)
        {
            var userDirectory = GetUserDirectory(username);
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
                catch (Exception)
                {
                    // Log error in production, skip corrupted files for now
                    continue;
                }
            }

            return workouts.OrderByDescending(w => w.LastModifiedDate ?? w.CreatedDate).ToList();
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
            catch (Exception)
            {
                // Log error in production
                return null;
            }
        }

        public async Task<Workout> CreateWorkoutAsync(string username, Workout workout)
        {
            workout.Id = Guid.NewGuid().ToString();
            workout.Username = username;
            workout.CreatedDate = DateTime.UtcNow;
            workout.LastModifiedDate = DateTime.UtcNow;

            var userDirectory = GetUserDirectory(username);
            Directory.CreateDirectory(userDirectory);

            var filePath = GetWorkoutFilePath(username, workout.Id);
            var json = JsonSerializer.Serialize(workout, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);

            return workout;
        }

        public async Task<Workout> UpdateWorkoutAsync(string username, string workoutId, Workout workout)
        {
            workout.Id = workoutId;
            workout.Username = username;
            workout.LastModifiedDate = DateTime.UtcNow;

            var filePath = GetWorkoutFilePath(username, workoutId);
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
            catch (Exception)
            {
                // Log error in production
                return Task.FromResult(false);
            }
        }

        public Task<bool> WorkoutExistsAsync(string username, string workoutId)
        {
            var filePath = GetWorkoutFilePath(username, workoutId);
            return Task.FromResult(File.Exists(filePath));
        }

        private string GetUserDirectory(string username)
        {
            return Path.Combine(_baseDirectory, username);
        }

        private string GetWorkoutFilePath(string username, string workoutId)
        {
            return Path.Combine(GetUserDirectory(username), $"{workoutId}.json");
        }
    }
}