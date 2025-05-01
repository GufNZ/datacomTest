# Job Application Tracker

A full-stack job application tracking system built with React and .NET Core Web API.

## Features

- View all job applications in a paginated table
- Add new job applications
- Edit existing applications
- Track application status (Applied/Interview/Offer/Rejected)
- Responsive UI with Material-UI components
- Pagination support (10 items per page)
- Type-safe TypeScript implementation

## Tech Stack

- Frontend: React 19.1.0 with TypeScript
- UI Framework: Material-UI v7.0.2
- State Management: React Hooks
- API Client: Axios v1.9.0
- Backend: .NET Core Web API v9.0
- Database: SQLite
- Testing: Jest v29.5.14, React Testing Library v16.1.0

## Getting Started

### Prerequisites

- Node.js (v22.15.3 or higher)
- .NET Core SDK (v9.0 or higher)
- yarn (v1.22.21 or higher)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/GufNZ/datacomTest.git
```

2. Build the backend:
```bash
dotnet build
```

3. Install frontend dependencies:
```bash
cd client
yarn install
```

### Running the Application

1. Start the backend:
```bash
dotnet run --project DatacomTest.Api/DatacomTest.Api.csproj
```

2. In a new terminal, start the frontend:
```bash
cd client
yarn start
```

The application will be available at `http://localhost:3000`

## Project Structure

```
client/
├── src/
│   ├── components/     # Reusable UI components
│   │   ├── AddApplicationForm.tsx
│   │   ├── EditApplicationForm.tsx
│   │   └── JobApplicationsList.tsx
│   ├── pages/         # Page components
│   │   ├── AddApplication.tsx
│   │   ├── EditApplication.tsx
│   │   └── Home.tsx
│   ├── services/      # API services
│   │   └── api.ts
│   └── types/         # TypeScript type definitions
│       ├── ApplicationStatus.ts
│       └── JobApplication.ts
├── public/
└── package.json

DatacomTest.Api/      # ASP.NET Core Web API backend
├── Controllers/      # API controllers
├── Models/          # Data models
├── Services/        # Business logic services
├── Tests/          # Integration tests
└── Program.cs      # Entry point

DatacomTest.Api.Tests/ # Unit and integration tests
└── DatacomTest.Api.Tests.csproj
```

## Assumptions

1. The application uses an in-memory SQLite database for simplicity
2. All dates are stored and displayed in ISO format
3. The frontend communicates with the backend via REST API endpoints
4. Pagination is implemented with 10 items per page
5. Status updates are immediate without confirmation

## API Documentation

The API is documented using Swagger UI, which is available at `http://localhost:5251/swagger` when the backend is running.

## API Endpoints

- `GET /api/applications` - Get paginated list of applications
- `GET /api/applications/{id}` - Get single application
- `POST /api/applications` - Create new application
- `PUT /api/applications/{id}` - Update application

## License

Public Domain
