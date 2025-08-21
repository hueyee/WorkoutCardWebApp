using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkoutCardWebApp.Shared.Services;
using WorkoutCardWebApp.Functions.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        // Register storage service - using in-memory for now, can be replaced with Azure Storage
        services.AddSingleton<IWorkoutStorageService, InMemoryWorkoutStorageService>();
    })
    .Build();

host.Run();