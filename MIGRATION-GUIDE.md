# Workout Card Web App - Azure Functions + Static Web Apps Migration

## Architecture Overview

This project has been migrated from a Blazor Server application to a modern Azure-native architecture:

### Before (Blazor Server)
- **Frontend**: Blazor Server with interactive server-side rendering
- **Backend**: ASP.NET Core API Controllers 
- **Storage**: Disk-based file storage
- **Deployment**: Attempted Azure Static Web Apps (incompatible with server-side requirements)

### After (Azure Functions + Blazor WebAssembly)
- **Frontend**: Blazor WebAssembly (client-side, runs in browser)
- **Backend**: Azure Functions (serverless API)
- **Storage**: In-memory storage (easily replaceable with Azure Storage/Cosmos DB)
- **Deployment**: Azure Static Web Apps with integrated Functions

## Project Structure

```
WorkoutCardWebApp/
├── WorkoutCardWebApp.Shared/          # Shared models and interfaces
│   ├── Models/                        # Workout, Block, Exercise, Set models
│   └── Services/                      # IWorkoutStorageService interface
├── WorkoutCardWebApp.Functions/       # Azure Functions API
│   ├── Services/                      # InMemoryWorkoutStorageService
│   ├── WorkoutFunctions.cs           # HTTP-triggered functions
│   ├── Program.cs                     # Function app configuration
│   ├── host.json                      # Functions runtime configuration
│   └── local.settings.json           # Local development settings
├── WorkoutCardWebApp.Client/          # Blazor WebAssembly app
│   ├── Services/                      # ApiWorkoutStorageService (calls Functions)
│   ├── Pages/                         # Workout pages (Workouts.razor, WorkoutEdit.razor)
│   ├── Layout/                        # App layout components
│   ├── Program.cs                     # Client app configuration
│   └── wwwroot/                       # Static web assets
└── .github/workflows/                 # Updated deployment workflow
```

## API Endpoints

All endpoints are implemented as Azure Functions:

- `GET /api/workouts/{username}` - Get all workouts for a user
- `GET /api/workouts/{username}/{workoutId}` - Get specific workout
- `POST /api/workouts/{username}` - Create new workout
- `PUT /api/workouts/{username}/{workoutId}` - Update existing workout
- `DELETE /api/workouts/{username}/{workoutId}` - Delete workout

## Development Setup

### Prerequisites
- .NET 8 SDK
- Azure Functions Core Tools (for local Functions development)

### Running Locally

1. **Start the Functions API** (Terminal 1):
   ```bash
   cd WorkoutCardWebApp.Functions
   func start
   ```
   The API will be available at `http://localhost:7071`

2. **Start the Blazor WebAssembly client** (Terminal 2):
   ```bash
   cd WorkoutCardWebApp.Client
   dotnet run
   ```
   The app will be available at `http://localhost:5177`

### Building

```bash
# Build Functions
dotnet build WorkoutCardWebApp.Functions/WorkoutCardWebApp.Functions.csproj

# Build Client
dotnet build WorkoutCardWebApp.Client/WorkoutCardWebApp.Client.csproj

# Build all projects
dotnet build
```

## Deployment

The deployment is configured for Azure Static Web Apps with integrated Azure Functions:

1. **GitHub Actions Workflow**: `.github/workflows/azure-static-web-apps-lemon-stone-01fc2311e.yml`
2. **App Location**: `/WorkoutCardWebApp.Client` (Blazor WebAssembly app)
3. **API Location**: `/WorkoutCardWebApp.Functions` (Azure Functions)
4. **Output Location**: `wwwroot` (Blazor build output)

### Configuration

The client automatically detects the environment:
- **Local Development**: API calls go to `https://localhost:7071/`
- **Azure Static Web Apps**: API calls go to `/api` (automatically routed to Functions)

## Storage Options

### Current: In-Memory Storage
- **Pros**: Simple, fast, no external dependencies
- **Cons**: Data is lost when Functions restart, not suitable for production

### Recommended: Azure Table Storage
```csharp
// Replace InMemoryWorkoutStorageService with:
services.AddSingleton<IWorkoutStorageService, AzureTableWorkoutStorageService>();
```

### Alternative: Azure Cosmos DB
```csharp
// For more complex scenarios:
services.AddSingleton<IWorkoutStorageService, CosmosDbWorkoutStorageService>();
```

## Migration Benefits

1. **Scalability**: Azure Functions scale automatically based on demand
2. **Cost Efficiency**: Pay only for what you use (Functions + Static Web Apps)
3. **Performance**: Client-side rendering reduces server load
4. **Reliability**: Azure-managed infrastructure with built-in redundancy
5. **Modern Architecture**: Separation of concerns, microservices-ready

## Testing

The migration has been tested to ensure:
- ✅ All projects build successfully
- ✅ Blazor WebAssembly client loads and displays UI correctly
- ✅ API calls are properly configured (tested with network errors)
- ✅ Error handling works gracefully
- ✅ Navigation between pages works
- ✅ Deployment workflow is updated for new structure

## Next Steps

1. **Replace in-memory storage** with Azure Table Storage or Cosmos DB
2. **Add authentication** using Azure AD B2C or similar
3. **Implement caching** for better performance
4. **Add monitoring** with Application Insights
5. **Add tests** for Functions and client components

## Troubleshooting

### Functions Not Starting
- Ensure Azure Functions Core Tools are installed
- Check `local.settings.json` configuration
- Verify .NET 8 SDK is installed

### Client API Calls Failing
- Check the API base URL configuration in `appsettings.json`
- Ensure Functions are running on the expected port
- Verify CORS configuration if needed

### Deployment Issues
- Check Azure Static Web Apps configuration
- Verify GitHub Actions secrets are configured
- Review build logs in GitHub Actions