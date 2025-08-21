using Microsoft.AspNetCore.Mvc;
using WorkoutCardWebApp.Models;
using WorkoutCardWebApp.Services;

namespace WorkoutCardWebApp.Controllers
{
    [ApiController]
    [Route("api/workouts")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutStorageService _storageService;

        public WorkoutsController(IWorkoutStorageService storageService)
        {
            _storageService = storageService;
        }

        // GET /api/workouts/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<List<Workout>>> GetWorkouts(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username is required");
            }

            var workouts = await _storageService.GetWorkoutsAsync(username);
            return Ok(workouts);
        }

        // GET /api/workouts/{username}/{workoutId}
        [HttpGet("{username}/{workoutId}")]
        public async Task<ActionResult<Workout>> GetWorkout(string username, string workoutId)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
            {
                return BadRequest("Username and workout ID are required");
            }

            var workout = await _storageService.GetWorkoutAsync(username, workoutId);
            if (workout == null)
            {
                return NotFound($"Workout {workoutId} not found for user {username}");
            }

            return Ok(workout);
        }

        // POST /api/workouts/{username}
        [HttpPost("{username}")]
        public async Task<ActionResult<Workout>> CreateWorkout(string username, [FromBody] Workout workout)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdWorkout = await _storageService.CreateWorkoutAsync(username, workout);
                return CreatedAtAction(
                    nameof(GetWorkout),
                    new { username = username, workoutId = createdWorkout.Id },
                    createdWorkout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating workout: {ex.Message}");
            }
        }

        // PUT /api/workouts/{username}/{workoutId}
        [HttpPut("{username}/{workoutId}")]
        public async Task<ActionResult<Workout>> UpdateWorkout(string username, string workoutId, [FromBody] Workout workout)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
            {
                return BadRequest("Username and workout ID are required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingWorkout = await _storageService.GetWorkoutAsync(username, workoutId);
            if (existingWorkout == null)
            {
                return NotFound($"Workout {workoutId} not found for user {username}");
            }

            try
            {
                // Preserve creation date
                workout.CreatedDate = existingWorkout.CreatedDate;
                var updatedWorkout = await _storageService.UpdateWorkoutAsync(username, workoutId, workout);
                return Ok(updatedWorkout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating workout: {ex.Message}");
            }
        }

        // DELETE /api/workouts/{username}/{workoutId}
        [HttpDelete("{username}/{workoutId}")]
        public async Task<ActionResult> DeleteWorkout(string username, string workoutId)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
            {
                return BadRequest("Username and workout ID are required");
            }

            var deleted = await _storageService.DeleteWorkoutAsync(username, workoutId);
            if (!deleted)
            {
                return NotFound($"Workout {workoutId} not found for user {username}");
            }

            return NoContent();
        }
    }
}