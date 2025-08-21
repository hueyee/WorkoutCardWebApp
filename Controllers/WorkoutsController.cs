using Microsoft.AspNetCore.Mvc;
using WorkoutCardWebApp.Models;
using WorkoutCardWebApp.Services;

namespace WorkoutCardWebApp.Controllers;

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
            return NotFound();
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

        if (workout == null)
        {
            return BadRequest("Workout data is required");
        }

        var createdWorkout = await _storageService.CreateWorkoutAsync(username, workout);
        return CreatedAtAction(nameof(GetWorkout), new { username, workoutId = createdWorkout.Id }, createdWorkout);
    }

    // PUT /api/workouts/{username}/{workoutId}
    [HttpPut("{username}/{workoutId}")]
    public async Task<ActionResult<Workout>> UpdateWorkout(string username, string workoutId, [FromBody] Workout workout)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
        {
            return BadRequest("Username and workout ID are required");
        }

        if (workout == null)
        {
            return BadRequest("Workout data is required");
        }

        var updatedWorkout = await _storageService.UpdateWorkoutAsync(username, workoutId, workout);
        if (updatedWorkout == null)
        {
            return NotFound();
        }

        return Ok(updatedWorkout);
    }

    // DELETE /api/workouts/{username}/{workoutId}
    [HttpDelete("{username}/{workoutId}")]
    public async Task<IActionResult> DeleteWorkout(string username, string workoutId)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
        {
            return BadRequest("Username and workout ID are required");
        }

        var deleted = await _storageService.DeleteWorkoutAsync(username, workoutId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}