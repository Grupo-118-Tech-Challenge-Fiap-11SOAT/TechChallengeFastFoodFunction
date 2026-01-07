# Tech Challenge Fast Food Function

An Azure Functions-based authentication and user management system for a fast food application, built with .NET 8 and C#.

## Overview

This project implements serverless Azure Functions to handle user authentication, employee management, and customer registration for a fast food ordering system. It provides JWT-based authentication with role-based access control and secure password hashing.

## Features

- **User Authentication**: JWT token-based login system
- **Employee Management**: Create and authenticate employees with roles
- **Customer Management**: Register and authenticate customers via CPF
- **Password Security**: HMACSHA512-based password hashing
- **Database Integration**: SQL Server connectivity for data persistence
- **Role-based Access**: Support for different user roles (Employee, Customer)

## Architecture

The solution follows a clean architecture pattern with the following components:

- **Functions**: HTTP-triggered Azure Functions for API endpoints
- **Models**: Data transfer objects and entity models
- **Repository**: Data access layer with SQL Server integration
- **Manager**: Business logic layer for authentication and user operations

## API Endpoints

### Authentication
- `POST /api/Login` - Authenticate users (employees via email/password or customers via CPF)

### User Management  
- `POST /api/CreateEmployeeFunction` - Create new employee accounts
- `POST /api/CreateCustomerFunction` - Register new customers

## Models

### Employee
- **Id**: Unique identifier
- **Name**: First name
- **Surname**: Last name  
- **Email**: Login email address
- **Password**: Encrypted password
- **Role**: Employee role/position
- **Cpf**: Brazilian tax ID
- **BirthDay**: Date of birth

### Customer
- **Id**: Unique identifier
- **Name**: First name
- **Surname**: Last name
- **Email**: Contact email
- **Cpf**: Brazilian tax ID (used for authentication)
- **BirthDay**: Date of birth

## Prerequisites

- .NET 8.0 SDK
- Azure Functions Core Tools
- SQL Server database
- Azure subscription (for deployment)

## Configuration

The application requires the following environment variables:

```bash
SqlConnectionString=<your-sql-server-connection-string>
JwtKey=<your-jwt-secret-key>
JwtExpirationMinutes=<token-expiration-time>
JwtIssuer=<jwt-issuer>
JwtAudience=<jwt-audience>
SecurityKey=<password-hashing-secret>
```

## Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/Grupo-118-Tech-Challenge-Fiap-11SOAT/TechChallengeFastFoodFunction.git
   cd TechChallengeFastFoodFunction
   ```

2. **Install dependencies**
   ```bash
   cd TechChallengeFastFoodFunction
   dotnet restore
   ```

3. **Create local.settings.json**
   ```json
   {
     "IsEncrypted": false,
     "Values": {
       "AzureWebJobsStorage": "UseDevelopmentStorage=true",
       "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
       "SqlConnectionString": "your-connection-string",
       "JwtKey": "your-jwt-key",
       "JwtExpirationMinutes": "60",
       "JwtIssuer": "your-issuer",
       "JwtAudience": "your-audience",
       "SecurityKey": "your-security-key"
     }
   }
   ```

4. **Run locally**
   ```bash
   func start --port 7063
   ```

## Usage Examples

### Employee Login
```bash
curl -X POST https://your-function-app.azurewebsites.net/api/Login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "employee@example.com",
    "password": "password123"
  }'
```

### Customer Login (via CPF)
```bash
curl -X POST https://your-function-app.azurewebsites.net/api/Login \
  -H "Content-Type: application/json" \
  -d '{
    "cpf": "12345678901"
  }'
```

### Create Employee
```bash
curl -X POST https://your-function-app.azurewebsites.net/api/CreateEmployeeFunction \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John",
    "surname": "Doe",
    "email": "john.doe@company.com",
    "password": "securePassword",
    "role": "Manager",
    "cpf": "12345678901",
    "birthDay": "1990-01-01"
  }'
```

## Deployment

The project includes GitHub Actions workflow for automatic deployment to Azure:

1. Configure Azure credentials in repository secrets
2. Push to `master` branch triggers deployment
3. The workflow builds and deploys to Azure Function App

## Security Features

- **JWT Authentication**: Secure token-based authentication
- **Password Hashing**: HMACSHA512 with secret key
- **SQL Injection Protection**: Parameterized queries
- **Role-based Access**: Different access levels for employees and customers

## Dependencies

- Microsoft.Azure.Functions.Worker (2.0.0)
- Microsoft.Data.SqlClient (6.1.1)
- System.IdentityModel.Tokens.Jwt (8.14.0)
- Microsoft.ApplicationInsights.WorkerService (2.23.0)

## Contributing

This project is part of the Tech Challenge from FIAP. For contributions, please follow the established patterns and ensure all tests pass.

## License

This project is part of an educational challenge and follows the institution's guidelines.

## Team

Grupo 118 - Tech Challenge FIAP - 11SOAT