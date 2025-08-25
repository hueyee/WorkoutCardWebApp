# Workout Card Web App - Azure Web App Architecture

## Architecture Overview

This project uses a traditional Azure Web App architecture with Blazor Server:

### Current Architecture (Azure Web App)
- **Frontend**: Blazor Server with interactive server-side rendering
- **Backend**: ASP.NET Core Web API Controllers 
- **Storage**: Disk-based file storage (configurable)
- **Deployment**: Azure Web App

### Benefits of This Architecture
- **Simpler Deployment**: Single web application deployment
- **Better SEO**: Server-side rendering provides better search engine optimization
- **Real-time Interactivity**: SignalR-based Blazor Server for responsive UI
- **Proven Technology Stack**: Traditional ASP.NET Core patterns
- **Cost Effective**: Single app service plan instead of multiple services
- **Easier Development**: No need for separate API and client projects

## Project Structure

```
WorkoutCardWebApp/
├── Components/                          # Blazor Server components
│   ├── Layout/                         # App layout components
│   │   ├── MainLayout.razor            # Main application layout
│   │   ├── NavMenu.razor               # Navigation menu
│   │   └── MainLayout.razor.css        # Layout styles
│   ├── Pages/                          # Page components
│   │   ├── Home.razor                  # Home page
│   │   ├── Workouts.razor              # Workout list page
│   │   └── WorkoutEdit.razor           # Workout editing page
│   ├── App.razor                       # Root app component
│   ├── Routes.razor                    # Route configuration
│   └── _Imports.razor                  # Global using statements
├── Controllers/                        # Web API controllers
│   └── WorkoutsController.cs           # REST API for workouts
├── Models/                            # Data models
│   ├── Block.cs                       # Workout block model
│   ├── Exercise.cs                    # Exercise model
│   ├── Set.cs                         # Exercise set model
│   └── Workout.cs                     # Workout model
├── Services/                          # Business logic services
│   ├── IWorkoutStorageService.cs      # Storage service interface
│   └── DiskBasedWorkoutStorageService.cs # File-based storage implementation
├── wwwroot/                           # Static web assets
│   ├── css/                           # Stylesheets
│   ├── js/                            # JavaScript files
│   └── favicon.ico                    # Site icon
├── Program.cs                         # Application startup configuration
├── appsettings.json                   # Application configuration
└── .github/workflows/                 # Azure Web App deployment workflow
    └── azure-web-app-deploy.yml       # GitHub Actions deployment
```

## API Endpoints

All endpoints are implemented as ASP.NET Core Web API controllers:

- `GET /api/workouts/{username}` - Get all workouts for a user
- `GET /api/workouts/{username}/{workoutId}` - Get specific workout
- `POST /api/workouts/{username}` - Create new workout
- `PUT /api/workouts/{username}/{workoutId}` - Update existing workout
- `DELETE /api/workouts/{username}/{workoutId}` - Delete workout

## Development Setup

### Prerequisites
- .NET 8 SDK

### Running Locally

1. **Start the application**:
   ```bash
   dotnet run
   ```
   The app will be available at `http://localhost:5205` (or check console for actual port)

### Building

```bash
# Build the application
dotnet build

# Build for production
dotnet build --configuration Release

# Publish for deployment
dotnet publish --configuration Release --output ./publish
```

## Deployment

The deployment is configured for Azure Web App:

1. **GitHub Actions Workflow**: `.github/workflows/azure-web-app-deploy.yml`
2. **Build Process**: Standard .NET build and publish
3. **Deploy Target**: Azure Web App using publish profile

### Configuration

The application automatically works in any environment:
- **Local Development**: Uses local file storage in `Workouts/` directory
- **Azure Web App**: Uses the same file storage (can be configured for Azure Storage)

### Environment Variables

Configure these in Azure Web App settings:
- `ASPNETCORE_ENVIRONMENT` - Set to "Production" for production environment
- Connection strings for database storage (if implemented)

## Storage Options

### Current: Disk-Based File Storage
- Simple JSON file storage in `Workouts/{username}/` directories
- Suitable for development and small-scale production
- Files persist across application restarts

### Production Options

#### Azure Blob Storage
```csharp
services.AddSingleton<IWorkoutStorageService, AzureBlobWorkoutStorageService>();
```

#### Azure Table Storage
```csharp
services.AddSingleton<IWorkoutStorageService, AzureTableWorkoutStorageService>();
```

#### Azure Cosmos DB
```csharp
services.AddSingleton<IWorkoutStorageService, CosmosDbWorkoutStorageService>();
```

## Migration Benefits

### Simplified Architecture
- **Single Deployment Unit**: One application instead of separate client and API
- **Reduced Complexity**: No need to manage Function Apps and Static Web Apps separately
- **Traditional Patterns**: Uses well-established ASP.NET Core patterns

### Better Performance
- **Server-Side Rendering**: Faster initial page loads
- **Real-time Updates**: SignalR integration for live updates
- **Reduced Latency**: No API calls for UI interactions

### Cost Optimization
- **Single App Service Plan**: Instead of separate Static Web App + Function App
- **Predictable Costs**: Standard web app pricing model
- **No Cold Starts**: Unlike Functions, web apps stay warm

## Testing

The application has been tested to ensure:
- ✅ Application builds successfully
- ✅ Blazor Server components load and render correctly
- ✅ API controllers respond correctly
- ✅ File storage works for workout data
- ✅ Navigation between pages works
- ✅ Real-time UI updates function properly

## Troubleshooting

### Application Not Starting
- Ensure .NET 8 SDK is installed
- Check `appsettings.json` for configuration issues
- Verify file permissions for `Workouts/` directory

### API Calls Failing
- Check that controllers are properly registered in `Program.cs`
- Verify routing configuration
- Check for CORS issues if accessing from external clients

### Deployment Issues
- Verify Azure Web App configuration
- Check GitHub Actions secrets for publish profile
- Review build logs in GitHub Actions
