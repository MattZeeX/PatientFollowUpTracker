# Patient Follow-Up Task Tracker API

A healthcare-themed ASP.NET Core Web API for managing mock follow-up tasks after patient appointments. It focuses on the backend pieces of a small data-centric workflow: REST endpoints, EF Core persistence, SQLite migrations, validation, audit logging, tests, and CI.

This project uses fake demonstration data only. It does not include fields for patient names, dates of birth, addresses, phone numbers, MRNs, or other direct identifiers.

## Highlights

- Built with C#/.NET 10, ASP.NET Core, EF Core, and SQLite
- Implements REST endpoints for task listing, creation, status updates, overdue tasks, and audit events
- Uses DTOs, validation, EF Core migrations, seed data, and Swagger/OpenAPI
- Records audit events when tasks are created or status changes
- Includes xUnit tests and GitHub Actions CI
- Uses fake `DEMO-*` reference codes instead of direct patient identifiers

## Purpose

The API models a small clinical operations workflow: tracking follow-up tasks that may need attention after an appointment, such as lab review, appointment reminders, referral follow-up, or medication review.

The project is intentionally compact. It is meant to show clear backend fundamentals rather than simulate a full production clinical system.

## Healthcare Data Safety Notice

This project is for demonstration only and is not intended for real patient data.

The API intentionally avoids storing direct patient identifiers. Demo tasks use fake reference codes such as:

```text
DEMO-001
DEMO-002
```

Avoid entering names, dates of birth, addresses, phone numbers, MRNs, insurance identifiers, or real clinical details into this application.

## Features

- REST API for mock follow-up task workflows
- SQLite persistence through Entity Framework Core
- EF Core migrations and fake demo seed data
- Swagger/OpenAPI UI for local API exploration
- DTOs for request and response contracts
- Validation for required fields and fake patient reference codes
- Audit events for task creation and status changes
- Overdue task query
- xUnit behavior tests
- GitHub Actions workflow for restore, build, and test

## API Endpoints

| Method | Path | Description |
| --- | --- | --- |
| `GET` | `/api/tasks` | List follow-up tasks ordered by due date |
| `GET` | `/api/tasks/{id}` | Get one follow-up task by ID |
| `GET` | `/api/tasks/overdue` | List overdue tasks that are not completed or cancelled |
| `POST` | `/api/tasks` | Create a new follow-up task |
| `PATCH` | `/api/tasks/{id}/status` | Update a task's status |
| `GET` | `/api/audit-events` | List audit events ordered newest first |

## Tech Stack

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger/OpenAPI via Swashbuckle
- xUnit
- GitHub Actions

The stack was chosen to keep the project small while still demonstrating common backend patterns: ASP.NET Core for REST APIs, EF Core for data access, SQLite for simple local persistence, Swagger/OpenAPI for interactive API exploration, and xUnit/GitHub Actions for automated verification.

## Getting Started

### Prerequisites

- .NET 10 SDK

### Restore and Build

From the repository root:

```powershell
dotnet restore
dotnet build
```

### Create or Update the Local Database

SQLite database files are generated locally and are not committed to the Git repository.

Run:

```powershell
dotnet ef database update --project PatientFollowUp.Api
```

This applies the EF Core migrations and creates `patient-followup.db` if it does not already exist.

### Run the API

For local development:

```powershell
dotnet run --project PatientFollowUp.Api --launch-profile https
```

The `https` launch profile starts both the HTTPS and HTTP URLs configured in `launchSettings.json`.

Swagger UI is available in the Development environment at:

```text
https://localhost:7135/swagger
```

Your local port may differ if launch settings are changed.

If your browser warns about the local HTTPS certificate, either use HTTP on port 5067 or trust the ASP.NET Core development certificate:

```powershell
dotnet dev-certs https --trust
```

HTTP Swagger URL when using the local HTTP endpoint:

```text
http://localhost:5067/swagger
```

## Example Requests

Create a follow-up task:

```json
{
  "patientReferenceCode": "DEMO-003",
  "taskType": "ReferralFollowUp",
  "description": "Coordinate demonstration referral follow-up task.",
  "dueDate": "2026-05-25"
}
```

Update task status:

```json
{
  "status": "Completed"
}
```

Allowed task types:

```text
LabReview
AppointmentReminder
ReferralFollowUp
MedicationReview
```

Allowed statuses:

```text
Open
InProgress
Completed
Cancelled
```

## Testing

Run all tests:

```powershell
dotnet test
```

The test project uses EF Core's in-memory provider for fast behavior tests. Current coverage verifies that:

- creating a valid task stores the task and creates an audit event
- updating task status changes the task and creates an audit event

## Continuous Integration

GitHub Actions runs on pushes and pull requests to `main`.

The workflow:

- restores dependencies
- builds the solution in `Release`
- runs the test suite

Workflow file:

```text
.github/workflows/dotnet.yml
```

## Design Notes

### DTOs

The API uses DTOs to separate HTTP contracts from EF Core entities. This keeps database persistence concerns separate from the JSON shape returned to API clients.

### Audit Logging

Audit events are written when a task is created or when its status changes. The audit log records the entity type, entity ID, action, timestamp, and details.

### Dates and Times

System timestamps use UTC:

- `CreatedAt`
- `UpdatedAt`
- audit event `Timestamp`

Task due dates use `DateOnly` because a due date is a calendar date, not a precise moment in time.

### Demo Data and PHI Avoidance

Seed data uses fake `DEMO-*` patient reference codes. The project does not include fields for names, birth dates, addresses, phone numbers, or MRNs.

### Mapping Tradeoff

For this small demo API, task entities are mapped to response DTOs in memory for readability. In a larger API with wider tables or higher volume, reusable EF projection expressions could reduce loaded columns and improve query efficiency.

### Environment Behavior

Swagger is enabled only in the Development environment. Published or hosted deployments should run with `ASPNETCORE_ENVIRONMENT=Production` or rely on ASP.NET Core's default Production environment.
