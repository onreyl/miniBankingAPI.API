# miniBankingAPI.API

A RESTful banking API built with .NET 8 that handles basic banking operations including account management, deposits, withdrawals, and money transfers.

## Tech Stack

- .NET 8
- Entity Framework Core
- SQL Server
- MediatR (CQRS pattern)
- FluentValidation
- Swagger/OpenAPI
- xUnit & Moq (Testing)

## Architecture

The project follows Clean Architecture principles with clear separation of concerns:

- API Layer: Controllers and middleware
- Application Layer: Business logic, CQRS handlers, DTOs
- Domain Layer: Entities, enums, interfaces
- Infrastructure Layer: Database context, repositories, migrations
- Tests Layer: Unit tests with Moq

## Features

- Create and manage bank accounts (TRY, USD, EUR)
- Deposit and withdraw money
- Transfer between accounts (same currency only)
- Transaction history tracking
- Account balance inquiries
- Input validation and error handling

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server

### Setup

1. Clone the repository
git clone https://github.com/yourusername/miniBankingAPI.git

2. Update connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "your-connection-string"
}

3. Apply migrations
dotnet ef database update

4. Run the application
dotnet run --project miniBankingAPI.API

The API will be available at https://localhost:7xxx with Swagger UI at /swagger

## API Endpoints

### Accounts
- POST /api/accounts - Create new account
- GET /api/accounts/{id} - Get account details
- GET /api/accounts - List all accounts
- DELETE /api/accounts/{id} - Delete account

### Transactions
- POST /api/transactions/deposit - Deposit money
- POST /api/transactions/withdraw - Withdraw money
- POST /api/transactions/transfer - Transfer between accounts
- GET /api/transactions/account/{accountId} - Get transaction history

## Testing

Run tests with:
dotnet test

Tests cover command handlers with mocked dependencies using Moq and FluentAssertions.

## Project Structure

miniBankingAPI/
├── miniBankingAPI.API/          # Web API layer
├── miniBankingAPI.Application/  # Business logic
├── miniBankingAPI.Domain/       # Domain entities
├── miniBankingAPI.Infrastructure/ # Data access
└── miniBankingAPI.Tests/        # Unit tests

## License

MIT
