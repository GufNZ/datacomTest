# datacomTest
## Full Stack .NET Developer Technical Test - Job Application Tracker

### Objective: Build a simple job application tracker with ASP.NET Core Web API and a React or Angular frontend.

### Scenario
You need to build a Job Application Tracker where users can add, update, and view job applications they have submitted. The system should keep track of jobs applied for, status updates, and the date applied.

### Requirements
#### Backend - ASP.NET Core Web API
- Create a RESTful API with the following endpoints:
  - GET /applications - Retrieve all job applications.
  - GET /applications/{id} - Retrieve a specific application.
  - POST /applications - Add a new application.
- Use Entity Framework Core (Code First) with an in-memory database or SQLite.
- Implement Repository Pattern and Dependency Injection.
- Bonus: Implement Swagger UI for API documentation.

#### Frontend - React or Angular
- Create a simple UI that allows users to:
  - List all job applications.
  - Add a new application.
  - Update an application (e.g., change status to Interview/Offer/Rejected).
- Use Axios (React) or HttpClient (Angular) for API communication.

- Display a table with:
  - Company Name
  - Position
  - Status
  - Date Applied
  - Actions (Edit)
- Implement a dropdown menu for updating job status.
- Bonus: Add pagination to the table.

#### Deliverables
- Backend: Working .NET Core Web API.
- Frontend: Simple UI in React or Angular.
- Instructions: A README.md file explaining:
  - How to run the backend and frontend.
  - Any assumptions made.

#### Evaluation Criteria
- Clean, well-structured, and modular code.
- Correct use of ASP.NET Core & Entity Framework Core.
- Use of async/await for async operations.
- API error handling and input validation.
- Well-structured React/Angular implementation.
- Basic styling and user-friendly UI.
