# **🧩 HabitForge — Full‑Stack Habit Tracking App**

HabitForge is a full‑stack habit‑tracking application built with ASP.NET Core, Vue, and SQL.
It focuses on clean architecture, modern UI, and actionable analytics to help users build consistent habits.

This project is part of my full‑stack engineering portfolio and demonstrates:

API design and backend architecture in C# / .NET

Frontend component design with Vue

Authentication, state management, and data persistence

Analytics and background processing (future integration with InsightWorker)

CI/CD, documentation, and production‑ready structure

# **📌 Features (Planned & In Progress)**

✔️ Core Features
- Create, edit, and delete habits

- Daily check‑ins

- Streak tracking

- Progress visualisation

- Responsive UI (mobile‑first)

# **🔐 Authentication**

- User registration & login

- JWT‑based authentication

- Secure API endpoints

# **📊 Analytics (Phase 2)**

- Completion trends

- Streak history

- Habit performance scoring

- Integration with InsightWorker (Python microservice)

# **🛠 Engineering Focus**

- Clean API architecture

- Entity Framework Core

- Repository & service layers

- Vue component architecture

- State management (Pinia or Vuex)

- CI pipeline (GitHub Actions)

- Docker support (planned)

# **🧱 Project Structure**
```Code
habitforge/
├── backend/        # ASP.NET Core Web API
├── frontend/       # Vue application
├── docs/           # Architecture notes, diagrams, decisions
│
├── .editorconfig
├── .gitignore
├── README.md
│
└── .github/
    └── workflows/
        └── ci.yml
```

This structure mirrors the template repo and keeps backend, frontend, and documentation cleanly separated.

# **🧭 Roadmap**

## **Phase 1 — Backend Foundations**

- ASP.NET Core project setup

- Habit entity + EF Core migrations

- CRUD endpoints

- JWT authentication

- Unit tests

## **Phase 2 — Frontend Foundations**

- Vue project setup

- Login/register pages

- Habit dashboard

- Check‑in UI

- API integration

## **Phase 3 — Analytics & Insights**

- Streak calculations

- Trend charts

- Integration with InsightWorker

- Background processing

## **Phase 4 — Polish & Deployment**

- CI/CD pipeline

- Docker support

- Azure deployment

- Documentation & screenshots

# **🧪 Tech Stack**

## **Backend**

- C#

- ASP.NET Core

- Entity Framework Core

- SQL Server or PostgreSQL

- JWT Authentication

## **Frontend**

- Vue

- TypeScript (optional)

- Pinia / Vuex

- TailwindCSS or custom CSS

## **Tooling**

- GitHub Actions

- Docker (planned)

- .editorconfig

- REST API documentation (OpenAPI/Swagger)

# **🗺 Why HabitForge Exists**

This project is designed to demonstrate:

- Full‑stack engineering capability

- Clean, maintainable architecture

- Ability to build real‑world features end‑to‑end

- Integration of multiple technologies (C#, Vue, Python microservices)

- Professional documentation and project planning

- It’s the centrepiece of my portfolio and will evolve as I build out the rest of the ecosystem.

# **📎 Related Projects**

- InsightWorker — Python analytics microservice

- TaskFlow Engine — C# workflow automation engine

- KanbanCraft — Vue drag‑and‑drop Kanban board

- Project Template — Base structure for all full‑stack projects
