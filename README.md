# HabitForge

A full-stack habit-tracking application built with **ASP.NET Core** and **Vue 3**. Create habits, track daily completions, monitor streaks, and visualise progress through analytics charts.

## Quick Start (Docker)

```bash
git clone https://github.com/ellis-hall94/habitforge.git
cd habitforge
docker compose up --build
```

Open **http://localhost:3000** — register an account and start tracking habits.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | C# / ASP.NET Core 8, Entity Framework Core, SQLite |
| Frontend | Vue 3, TypeScript, Vite 6, Pinia, Chart.js |
| Auth | JWT (BCrypt password hashing) |
| Testing | xUnit, EF Core InMemory |
| CI/CD | GitHub Actions (build + test on PR) |
| Infrastructure | Docker, Docker Compose, nginx |

## Architecture

```
Browser → nginx (port 3000)
             ├── Static SPA files (Vue build)
             └── /api/* → ASP.NET Core API (port 8080)
                             ├── Controllers
                             ├── Services (business logic)
                             ├── EF Core DbContext
                             └── SQLite database
```

The frontend is served by nginx, which also reverse-proxies `/api/` requests to the backend container. The API follows a controller → service → EF Core architecture with JWT authentication on protected endpoints.

## Local Development

### Backend

```bash
cd backend
dotnet restore
dotnet run --project HabitForge.Api
```

API runs at **http://localhost:5000** with Swagger UI available in development mode.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Dev server runs at **http://localhost:5173** with a Vite proxy forwarding `/api` to the backend.

### Tests

```bash
cd backend
dotnet test
```

## API Endpoints

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/api/Health` | No | Health check |
| POST | `/api/Authentication/register` | No | Register user, returns JWT |
| POST | `/api/Authentication/login` | No | Login, returns JWT |
| GET | `/api/Habits` | Yes | Get all habits for user |
| GET | `/api/Habits/{id}` | Yes | Get single habit |
| POST | `/api/Habits` | Yes | Create habit |
| PUT | `/api/Habits/{id}` | Yes | Update habit |
| DELETE | `/api/Habits/{id}` | Yes | Delete habit |
| GET | `/api/Analytics/summary` | Yes | Overall stats and per-habit streaks |
| GET | `/api/Analytics/streaks` | Yes | All habit streaks |
| GET | `/api/Analytics/habits/{id}/streaks` | Yes | Single habit streak |
| GET | `/api/Analytics/habits/{id}/trends?days=30` | Yes | Daily completion trend data |

## Project Structure

```
habitforge/
├── docker-compose.yml
├── .github/workflows/ci.yml
├── backend/
│   ├── Dockerfile
│   ├── HabitForge.sln
│   ├── HabitForge.Api/
│   │   ├── Configuration/
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── Extensions/
│   │   ├── Middleware/
│   │   ├── Migrations/
│   │   ├── Models/
│   │   │   ├── Domain/
│   │   │   └── DTOs/
│   │   ├── Services/
│   │   └── Program.cs
│   └── HabitForge.Tests/
│       └── Services/
└── frontend/
    ├── Dockerfile
    ├── nginx.conf
    └── src/
        ├── components/
        ├── router/
        ├── services/
        ├── stores/
        └── views/
```

## Roadmap

- [x] **Phase 1** — Backend foundations (API, auth, CRUD, EF Core)
- [x] **Phase 2** — Frontend foundations (Vue 3, routing, Pinia stores, API integration)
- [x] **Phase 3** — Analytics and insights (streaks, trends, Chart.js visualisations)
- [x] **Phase 4** — Polish and deployment (Docker, CI/CD, tests, documentation)
