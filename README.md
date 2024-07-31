# APIAssessment

This project implements an API for managing users and their beneficiaries, including top-up functionalities. The API is built using .NET Core and follows the Clean Architecture principles. It utilizes MediatR for handling commands and queries, Entity Framework Core for data access, and implements the repository pattern for better separation of concerns. The project also includes unit tests to ensure the reliability of the business logic.

## Project Structure
-**Assessment.Api**: The main entry point for the API project. Contains controllers and API configurations.
-**Assessment.Core**: Contains the core logic of the application, including commands, queries, handlers, and DTOs.
-**Assessment.Domain**: Contains the domain models and repository interfaces.
-**Assessment.Infrastructure**: Contains the data access logic, including Entity Framework Core DbContext and repository implementations.

## Key Components

- **Controllers**: Handle HTTP requests and delegate to MediatR for command/query processing.
- **MediatR**: Used to handle commands and queries in a clean, decoupled manner.
- **Entity Framework Core**: ORM used for data access and migrations.
- **Clean Architecture**: Ensures separation of concerns and keeps the codebase maintainable and testable.

## Prerequisites

- .NET Core SDK 6.0 or later
- SQL Server (or any other compatible database)
- Entity Framework Core tools
