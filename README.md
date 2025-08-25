# Workout Card Web App

A modern Azure-native web application for managing workout routines, built with Blazor Server and ASP.NET Core.

## 🏗️ Architecture

**Frontend**: Blazor Server (server-side rendering with interactive components)  
**Backend**: ASP.NET Core Web API Controllers  
**Storage**: Disk-based file storage (easily replaceable with Azure Storage/Cosmos DB)  
**Deployment**: Azure Web App

## ✨ Features

- **Simple Username-Based Access**: No authentication required, just enter a username
- **Server-Side Rendering**: Fast, SEO-friendly Blazor Server interface with real-time interactivity
- **Traditional Web Architecture**: Reliable ASP.NET Core Web API with proven patterns
- **REST API**: Full CRUD operations via ASP.NET Core controllers
- **Mobile Compatible**: JSON format compatible with mobile applications
- **Azure-Native**: Designed for Azure Web App deployment

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WorkoutCardWebApp
   ```

2. **Run the application**
   ```bash
   dotnet run
   ```
   App available at `http://localhost:5205` (or check console output for actual port)

### Using the Application

1. **Navigate to "My Workouts"** from the navigation menu
2. **Enter a username** in the textbox
3. **Click "Load Workouts"** to see existing workouts for that user
4. **Create workouts** using the "Create New Workout" button
5. **Edit or delete** workouts using the buttons on each workout card

## 📡 API Endpoints

All endpoints are implemented as ASP.NET Core Web API controllers:

- `GET /api/workouts/{username}` - Get all workouts for a user
- `GET /api/workouts/{username}/{workoutId}` - Get a specific workout
- `POST /api/workouts/{username}` - Create a new workout
- `PUT /api/workouts/{username}/{workoutId}` - Update a workout
- `DELETE /api/workouts/{username}/{workoutId}` - Delete a workout

## 📊 Data Models

### Workout
- `Id`, `Name`, `Description`
- `Blocks` (list of workout blocks)
- `CreatedAt`, `UpdatedAt`
- `Difficulty`, `Tags`, `Notes`

### Block
- `Id`, `Name`, `Description`
- `Exercises` (list of exercises)
- `Sets` (number of block repetitions)
- `RestTime`, `Notes`

### Exercise
- `Id`, `Name`, `Description`, `Category`
- `Sets` (list of individual sets)
- `Notes`, `RestTime`

### Set
- `Reps`, `Weight`, `Duration`, `Distance`
- `Notes`, `Completed`

## 🏗️ Project Structure

```
WorkoutCardWebApp/
├── Components/                          # Blazor Server components
│   ├── Layout/                         # App layout components  
│   └── Pages/                          # Page components
├── Controllers/                        # Web API controllers
├── Models/                            # Data models
├── Services/                          # Business logic services
├── wwwroot/                           # Static web assets
└── .github/workflows/                 # Azure Web App deployment workflow
```

## 🚀 Deployment

Configured for Azure Web App deployment:

1. **GitHub Actions** automatically builds and deploys the web application
2. **Traditional Web App** - Standard ASP.NET Core hosting model
3. **Automatic HTTPS** and load balancing included

## 🔧 Storage Options

### Current: Disk-Based (Development)
- Fast and simple for development/testing
- Files stored in local `Workouts/` directory

### Production: Azure Table Storage (Recommended)
```csharp
services.AddSingleton<IWorkoutStorageService, AzureTableWorkoutStorageService>();
```

### Advanced: Azure Cosmos DB
```csharp
services.AddSingleton<IWorkoutStorageService, CosmosDbWorkoutStorageService>();
```

## 📚 Additional Documentation

- [Azure Web Apps](https://docs.microsoft.com/azure/app-service/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Blazor Server](https://docs.microsoft.com/aspnet/core/blazor/hosting-models#blazor-server)

## 🛠️ Technologies

- **Frontend**: Blazor Server, Bootstrap 5
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Storage**: Configurable (Disk-based, Azure Storage, Cosmos DB)
- **Deployment**: Azure Web App, GitHub Actions
- **Development**: .NET 8, Visual Studio Code/Visual Studio