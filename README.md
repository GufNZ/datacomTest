# Job Application Tracker

A full-stack application for tracking job applications built with ASP.NET Core Web API and React.

## Project Structure

```
datacomTest/
├── datacomTest.Api/           # ASP.NET Core Web API backend
├── frontend/                  # React frontend (coming soon)
└── README.md
```

## Prerequisites

- .NET 9.0 SDK
- Node.js (for frontend development)
- Yarn (Package Manager)

## Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd datacomTest.Api
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

The backend API will be available at `http://localhost:5251`.

## Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   yarn install
   ```

3. Start the development server:
   ```bash
   yarn start
   ```

## Features

- Track job applications with company name, position, status, and application date
- CRUD operations for job applications
- Status tracking (Applied, Interview, Offer, Rejected)
- RESTful API with Swagger documentation

## Tech Stack

- Backend: ASP.NET Core Web API
- Database: SQLite
- Frontend: React
- API Documentation: Swagger UI

## Development Status

- Backend setup completed
- Frontend setup in progress

## Next Steps

- Create the React UI components

## License

Public Domain
