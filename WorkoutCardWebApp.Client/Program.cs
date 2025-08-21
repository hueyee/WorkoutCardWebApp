using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WorkoutCardWebApp.Client;
using WorkoutCardWebApp.Client.Services;
using WorkoutCardWebApp.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HTTP client to point to Azure Functions API
// For local development, this will be localhost:7071; for production, it will be the Static Web App API
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrEmpty(apiBaseUrl))
{
    // For Azure Static Web Apps, the API is available at /api relative to the app URL
    apiBaseUrl = builder.HostEnvironment.BaseAddress;
}
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Register the API storage service
builder.Services.AddScoped<IWorkoutStorageService, ApiWorkoutStorageService>();

await builder.Build().RunAsync();
