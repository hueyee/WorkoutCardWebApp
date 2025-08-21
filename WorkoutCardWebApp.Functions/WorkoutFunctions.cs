using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using WorkoutCardWebApp.Shared.Models;
using WorkoutCardWebApp.Shared.Services;

namespace WorkoutCardWebApp.Functions;

public class WorkoutFunctions
{
    private readonly ILogger _logger;
    private readonly IWorkoutStorageService _storageService;

    public WorkoutFunctions(ILoggerFactory loggerFactory, IWorkoutStorageService storageService)
    {
        _logger = loggerFactory.CreateLogger<WorkoutFunctions>();
        _storageService = storageService;
    }

    [Function("GetWorkouts")]
    public async Task<HttpResponseData> GetWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workouts/{username}")] HttpRequestData req,
        string username)
    {
        _logger.LogInformation("Getting workouts for user: {Username}", username);

        if (string.IsNullOrWhiteSpace(username))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Username is required");
            return badRequest;
        }

        try
        {
            var workouts = await _storageService.GetWorkoutsAsync(username);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(workouts));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workouts for user: {Username}", username);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("GetWorkout")]
    public async Task<HttpResponseData> GetWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "workouts/{username}/{workoutId}")] HttpRequestData req,
        string username,
        string workoutId)
    {
        _logger.LogInformation("Getting workout {WorkoutId} for user: {Username}", workoutId, username);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Username and workout ID are required");
            return badRequest;
        }

        try
        {
            var workout = await _storageService.GetWorkoutAsync(username, workoutId);
            if (workout == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(workout));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workout {WorkoutId} for user: {Username}", workoutId, username);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("CreateWorkout")]
    public async Task<HttpResponseData> CreateWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "workouts/{username}")] HttpRequestData req,
        string username)
    {
        _logger.LogInformation("Creating workout for user: {Username}", username);

        if (string.IsNullOrWhiteSpace(username))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Username is required");
            return badRequest;
        }

        try
        {
            var body = await req.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Workout data is required");
                return badRequest;
            }

            var workout = JsonSerializer.Deserialize<Workout>(body);
            if (workout == null)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid workout data");
                return badRequest;
            }

            var createdWorkout = await _storageService.CreateWorkoutAsync(username, workout);
            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(createdWorkout));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workout for user: {Username}", username);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("UpdateWorkout")]
    public async Task<HttpResponseData> UpdateWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "workouts/{username}/{workoutId}")] HttpRequestData req,
        string username,
        string workoutId)
    {
        _logger.LogInformation("Updating workout {WorkoutId} for user: {Username}", workoutId, username);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Username and workout ID are required");
            return badRequest;
        }

        try
        {
            var body = await req.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Workout data is required");
                return badRequest;
            }

            var workout = JsonSerializer.Deserialize<Workout>(body);
            if (workout == null)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid workout data");
                return badRequest;
            }

            var updatedWorkout = await _storageService.UpdateWorkoutAsync(username, workoutId, workout);
            if (updatedWorkout == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(updatedWorkout));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workout {WorkoutId} for user: {Username}", workoutId, username);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("DeleteWorkout")]
    public async Task<HttpResponseData> DeleteWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "workouts/{username}/{workoutId}")] HttpRequestData req,
        string username,
        string workoutId)
    {
        _logger.LogInformation("Deleting workout {WorkoutId} for user: {Username}", workoutId, username);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(workoutId))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Username and workout ID are required");
            return badRequest;
        }

        try
        {
            var deleted = await _storageService.DeleteWorkoutAsync(username, workoutId);
            if (!deleted)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return req.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting workout {WorkoutId} for user: {Username}", workoutId, username);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}