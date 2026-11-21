# BookNest – Book Rental Management System

BookNest is a web-based book rental management system built with ASP.NET Core MVC and Clean Architecture.

This project is developed as a practical portfolio project to demonstrate backend development skills, software architecture, database design, and modern development practices.

## Technologies

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Git & GitHub

## Architecture

The solution follows Clean Architecture and is divided into four projects:

| Project | Responsibility |
|---|---|
| `BookNest.Web` | Presentation layer, MVC controllers and views |
| `BookNest.Application` | Application services, business use cases and interfaces |
| `BookNest.Domain` | Core business entities and domain logic |
| `BookNest.Infrastructure` | Database access, EF Core and external services |

## Main Features

Planned features include:

- User registration and authentication
- Book catalog and category management
- Book rental management
- Customer management
- Inventory management
- Admin dashboard

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 10 SDK
- SQL Server
- SQL Server Management Studio (optional)

### Database Configuration

Update the connection string in `BookNest.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BookNestDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Run the Application

1. Clone this repository.
2. Open `BookNest.slnx` in Visual Studio.
3. Configure the SQL Server connection string.
4. Run the application using the `BookNest.Web` startup project.

## Project Status

Currently under development.

## Author

Developed as a software development portfolio project.
