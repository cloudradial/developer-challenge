# Star Wars Movies Application

## Technology Stack

### Backend
- **.NET 9.0**
- **ASP.NET Core Web API**
- **Clean Architecture** (Domain, Application, Infrastructure, API)
- **Memory Caching** for API response optimization
- **Unit Testing**: xUnit, Moq, FluentAssertions

### Frontend
- **Angular 19** (latest version with standalone components)
- **RxJS** for reactive programming
- **TypeScript**
- **Plain CSS** for styling
- **HttpClient** for API communication

## Prerequisites

Before running this application, ensure you have the following installed:

- **.NET 9.0 SDK** (or .NET 8.0 minimum)
- **Node.js 18+** (with npm)
- **Angular CLI** (will be used via npx)

## Getting Started

### Quick Start (Using Helper Scripts)

**Option A - Windows:**
1. Double-click `start-backend.bat` to start the backend
2. Double-click `start-frontend.bat` to start the frontend (in a new terminal)

**Option B - Linux/Mac:**
```bash
./start-backend.sh &    # Start backend in background
./start-frontend.sh     # Start frontend
```

### Manual Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd developer-challenge
```

### 2. Run the Backend

Open a terminal and navigate to the backend directory:

```bash
cd backend
dotnet restore
dotnet run --project StarWarsMovies.Api
```

The backend API will start on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

You should see output similar to:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

### 3. Run the Frontend

Open a **new terminal** (keep the backend running) and navigate to the frontend directory:

```bash
cd frontend
npm install
npm start
```

The Angular application will start on `http://localhost:4200`

### 4. Access the Application

Open your browser and navigate to:
```
http://localhost:4200
```

## Running Tests

### Backend Unit Tests

```bash
cd backend
dotnet test
```

This will run all unit tests and display the results. You should see output like:
```
Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4
```

## API Endpoints

The backend provides the following REST API endpoints:

- `GET /api/movies` - Get all Star Wars movies with characters
- `GET /api/movies/{episodeId}` - Get a specific movie by episode ID

### Example Response

```json
{
  "episodeId": 4,
  "title": "A New Hope",
  "director": "George Lucas",
  "producer": "Gary Kurtz, Rick McCallum",
  "releaseDate": "May 25, 1977",
  "characters": [
    "Luke Skywalker",
    "C-3PO",
    "R2-D2",
    ...
  ]
}
```

## Acknowledgments

- Star Wars API (SWAPI): https://swapi.info/
- Star Wars content owned by Lucasfilm Ltd.
