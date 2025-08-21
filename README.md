# Workout Card Web App

A modern Azure-native web application for managing workout routines, built with Blazor WebAssembly and Azure Functions.

## 🏗️ Architecture

**Frontend**: Blazor WebAssembly (client-side rendering)  
**Backend**: Azure Functions (serverless API)  
**Storage**: In-memory (easily replaceable with Azure Storage/Cosmos DB)  
**Deployment**: Azure Static Web Apps with integrated Functions

## ✨ Features

- **Simple Username-Based Access**: No authentication required, just enter a username
- **Serverless Architecture**: Azure Functions for scalable, cost-effective API
- **Client-Side Rendering**: Fast, responsive Blazor WebAssembly interface
- **REST API**: Full CRUD operations via Azure Functions
- **Mobile Compatible**: JSON format compatible with mobile applications
- **Azure-Native**: Designed for Azure Static Web Apps deployment

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Azure Functions Core Tools (for local API development)

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WorkoutCardWebApp
   ```

2. **Start the Azure Functions API** (Terminal 1)
   ```bash
   cd WorkoutCardWebApp.Functions
   func start
   ```
   API available at `http://localhost:7071`

3. **Start the Blazor WebAssembly client** (Terminal 2)
   ```bash
   cd WorkoutCardWebApp.Client
   dotnet run
   ```
   App available at `http://localhost:5177`

### Using the Application

1. **Navigate to "My Workouts"** from the navigation menu
2. **Enter a username** in the textbox
3. **Click "Load Workouts"** to see existing workouts for that user
4. **Create workouts** using the "Create New Workout" button
5. **Edit or delete** workouts using the buttons on each workout card

## 📡 API Endpoints

All endpoints are implemented as Azure Functions:

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
├── WorkoutCardWebApp.Shared/          # Shared models and interfaces
├── WorkoutCardWebApp.Functions/       # Azure Functions API
├── WorkoutCardWebApp.Client/          # Blazor WebAssembly app
└── .github/workflows/                 # Azure deployment workflow
```

## 🚀 Deployment

Configured for Azure Static Web Apps with integrated Azure Functions:

1. **GitHub Actions** automatically builds and deploys both client and API
2. **Zero Configuration** - Azure Static Web Apps handles routing
3. **Automatic HTTPS** and global CDN included

## 🔧 Storage Options

### Current: In-Memory (Development)
- Fast and simple for development/testing
- Data resets when Functions restart

### Production: Azure Table Storage (Recommended)
```csharp
services.AddSingleton<IWorkoutStorageService, AzureTableWorkoutStorageService>();
```

### Advanced: Azure Cosmos DB
```csharp
services.AddSingleton<IWorkoutStorageService, CosmosDbWorkoutStorageService>();
```

## 📚 Additional Documentation

- [Migration Guide](MIGRATION-GUIDE.md) - Detailed migration documentation
- [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps/)
- [Azure Functions](https://docs.microsoft.com/azure/azure-functions/)

## 🛠️ Technologies

- **Frontend**: Blazor WebAssembly, Bootstrap 5
- **Backend**: Azure Functions (.NET 8, Isolated Worker)
- **Storage**: Configurable (In-Memory, Azure Storage, Cosmos DB)
- **Deployment**: Azure Static Web Apps, GitHub Actions
- **Development**: .NET 8, Visual Studio Code/Visual Studio