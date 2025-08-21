# WorkoutCardWebApp

A Blazor Server MVP application for managing workout routines with disk-based storage and REST API.

## Features

- **Simple Username-Based Access**: No authentication required, just enter a username
- **Disk-Based Storage**: Workouts stored as JSON files in `Workouts/{username}/{workoutId}.json`
- **REST API**: Full CRUD operations via API endpoints
- **Responsive UI**: Bootstrap-based Blazor Server interface
- **Mobile Compatible**: JSON format compatible with mobile applications
- **Lightweight Hosting**: No database required

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   dotnet run
   ```
4. Open your browser to `http://localhost:5000`

### Using the Application

1. **Navigate to "My Workouts"** from the navigation menu
2. **Enter a username** in the textbox
3. **Click "Load Workouts"** to see existing workouts for that user
4. **Create workouts** using the "Create New Workout" button
5. **Edit or delete** workouts using the buttons on each workout card

## REST API Endpoints

- `GET /api/workouts/{username}` - Get all workouts for a user
- `GET /api/workouts/{username}/{workoutId}` - Get a specific workout
- `POST /api/workouts/{username}` - Create a new workout
- `PUT /api/workouts/{username}/{workoutId}` - Update a workout
- `DELETE /api/workouts/{username}/{workoutId}` - Delete a workout

## Data Models

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

## File Storage

Workouts are stored in JSON format:
```
Workouts/
├── username1/
│   ├── workout-id-1.json
│   └── workout-id-2.json
└── username2/
    └── workout-id-3.json
```

## Architecture

- **Models**: Domain entities (Workout, Block, Exercise, Set)
- **Services**: `IWorkoutStorageService` with disk-based implementation
- **Controllers**: REST API endpoints (`WorkoutsController`)
- **Components**: Blazor Server pages and components
- **Storage**: JSON files with camelCase naming for mobile compatibility