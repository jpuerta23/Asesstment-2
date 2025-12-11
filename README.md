# Employee Management System

**Author:** JHon freddy puerta

**GitHub Repository:** [https://github.com/jpuerta23/Asesstment-2](https://github.com/jpuerta23/Asesstment-2)

## Overview

This is a comprehensive Employee Management System built with **.NET 8**, designed to manage employee records, authentication, and reporting. The solution follows a layered architecture and includes a Web MVC application, a RESTful API, and a comprehensive test suite.

## Key Features

### 🔐 Authentication & Security
- **JWT Authentication**: Secure API endpoints using JSON Web Tokens.
- **Role-Based Access Control (RBAC)**: Distinct roles for `Admin` and `Customer` (Employee).
- **Self-Registration**: Employees can register themselves, triggering an automatic welcome email.

### 👥 Employee Management
- **CRUD Operations**: Create, Read, Update, and Delete employee records.
- **Profile Management**: Employees can view their own profile and download their resume.
- **AI Integration**: Uses **Google Gemini AI** to generate professional profile suggestions based on role and education.

### 📄 Reporting & Exports
- **PDF Export**: Generate professional resumes (CVs) using **QuestPDF**.
- **Excel Export**: Export employee lists to Excel.

### 🛠 Technical Features
- **Docker Support**: Fully containerized solution (API, Web, Database).
- **Automated Tests**: Unit tests for critical authentication logic using **xUnit** and **Moq**.
- **Swagger UI**: Interactive API documentation.

## Technologies Used

- **Framework**: .NET 8 (ASP.NET Core Web API & MVC)
- **Database**: PostgreSQL (Entity Framework Core)
- **AI Service**: Google Gemini Pro
- **PDF Generation**: QuestPDF
- **Testing**: xUnit, Moq, EF Core InMemory
- **Containerization**: Docker, Docker Compose

## Project Structure

This solution follows **Clean Architecture** principles:

- **Test.Domain**: Core business entities and interfaces (no dependencies)
- **Test.Application**: Application services and business logic interfaces
- **Test.Infrastructure**: Data access, external services (EF Core, Repositories, Email, PDF, Excel, AI)
- **Test.Web**: MVC Web Application for the user interface
- **Test.Api**: RESTful API for authentication and data access (currently minimal)
- **Test.Tests**: Unit test project with xUnit and Moq

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL (if running locally without Docker)

### 🚀 Running with Docker (Recommended)

1.  Clone the repository.
2.  Navigate to the solution root.
3.  **Configure Environment**:
    - Copy `.env.example` to `.env`.
    - Update the variables in `.env` if necessary (e.g., for local DB or custom settings).
    - The project is pre-configured to use the **Render PostgreSQL database**.

4.  Run the following command:

    ```bash
    docker compose up --build
    ```

5.  Access the applications:
    - **Web App**: [http://localhost:5001](http://localhost:5001)
    - **API Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
    - **PostgreSQL**: The application now connects to Render by default. To use local DB, uncomment the postgres service in `docker-compose.yml`.

6.  **Default Login Credentials**:
    ```
    Username: admin
    Password: admin
    ```
    > ⚠️ **Note**: Change these credentials in production! The default admin user is seeded automatically on first run.

7.  To stop the containers:
    ```bash
    docker compose down
    ```

### 🏃 Running Locally

1.  **Configure Database**:
    - The project is configured to use the Render database in `appsettings.json`.
    - To use a local database, update `ConnectionStrings` in `appsettings.Development.json` for both `Test.Api` and `Test.Web`.

2.  **Apply Migrations**:
    ```bash
    dotnet ef database update --project Test.Infrastructure --startup-project Test.Web
    ```

3.  **Run API**:
    ```bash
    cd Test.Api
    dotnet run
    ```
4.  **Run Web App**:
    ```bash
    cd Test.Web
    dotnet run
    ```
5.  **Default Login Credentials**:
    ```
    Username: admin
    Password: admin
    ```

## Default Users

The system comes with a pre-seeded admin user:

| Username | Password | Role  |
|----------|----------|-------|
| admin    | admin    | Admin |

> ⚠️ **Security Warning**: These are default credentials for development only. In production, ensure you:
> - Change the default password immediately
> - Implement password hashing (currently passwords are stored in plain text for demo purposes)
> - Use environment variables for sensitive configuration

## API Documentation

The API is documented using Swagger. Once the API is running, visit `/swagger` to explore the endpoints.

- **Auth**: Login and Register endpoints.
- **Empleados**: Protected endpoints for employee data.
- **Departamentos**: Public endpoints for form data.

## Testing

To run the automated tests:

```bash
dotnet test
```

## Configuration

### Environment Variables
Ensure the following settings are configured in `appsettings.json` or `docker-compose.yml`:

- `ConnectionStrings:DefaultConnection`: PostgreSQL connection string.
- `Jwt:Key`: Secret key for token generation.
- `Gemini:ApiKey`: Google Gemini API Key for AI features.
- `EmailSettings`: SMTP configuration for sending emails.

---
*Developed by JHon freddy puerta*
