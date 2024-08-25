
# FundingSouq Assessment API

This repository contains a simple banking control panel API built with ASP.NET Core. The API is designed to manage client data, handle user registration and authentication, and provide a range of admin functionalities. This project is structured using Clean Architecture principles, following the CQRS pattern, and leveraging a variety of modern tools and technologies to ensure scalability, security, and maintainability.

The api is hosted on this route: [Funding Souq Assessment](http://funding-souq-assessment.runasp.net/index.html). However, you can clone the repository and follow the Getting Started guide below. 

## Getting Started

### Running the Project

The easiest way to run the project is via dotnet run command as follows:

1. **Clone the Repository:**
   ```bash
    git clone https://github.com/isl-mahrous/FundingSouq.Assessment
    cd FundingSouq.Assessment
    ```
2. **Run the Project:** 
   ```bash
   dotnet run --project FundingSouq.Assessment.Api
    ```
3. **Accessing the API:** 
   Once the application is running, you can access the API at `http://localhost:5257`. The Swagger UI is available at `http://localhost:5257`.


### To run it with Docker Compose. Follow these steps:

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/isl-mahrous/FundingSouq.Assessment
   cd FundingSouq.Assessment
   ```

2. **Set Environment Variables:**
    simply go to `appsettings.json` and un-comment the connection strings for both postgres and redis docker instances.

3. **Build and Run the Project:**
   Ensure Docker and Docker Compose are installed on your machine. Then run:
   ```bash
   docker-compose up --build
   ```
   This command will pull the necessary images, build the project, and start all services defined in the `docker-compose.yml` file.
   If it fails to run the first time, it could be because of timeout caused by database not ready yet and being pulled from docker hub. In that case, once all the images are pulled, run the command again. 

4. **Accessing the API:**
   Once the application is running, you can access the API at `http://localhost:5000`. The Swagger UI is available at `http://localhost:5000`.

### Admin User

An admin user is automatically created and seeded into the database when you run the application for the first time. You can use the following credentials to log in as an admin and perform authorized operations:

- **Email:** `admin@fundingsouq.com`
- **Password:** `admin@123`

Use these credentials to obtain a token and interact with protected endpoints.

## Project Features

### Clean Architecture

- **Separation of Concerns:** The project is organized into distinct layers: API, Application, Domain, and Infrastructure. This ensures a clear separation of business logic, data access, and presentation layers, making the codebase more maintainable and scalable.

### CQRS with MediatR

- **Command and Query Responsibility Segregation (CQRS):** We use MediatR to implement the CQRS pattern, ensuring that commands and queries are handled separately. This improves the scalability and flexibility of the system.
  
### PostgreSQL

- **Database:** PostgreSQL is used as the relational database to store client and user data. Entity Framework Core is used as the ORM for interacting with the database, ensuring efficient data access and manipulation.

### Redis Cache

- **Caching:** Redis is integrated to provide caching functionality, significantly improving the performance of frequently accessed data.

### Distributed Locks with Redis

- **Concurrency Control:** Redis is also used to implement distributed locks using RedLock, ensuring that certain operations are safely executed in a distributed environment.

### Role-Based and Policy-Based Authorization

- **Security:** The API uses ASP.NET Core's role-based and policy-based authorization mechanisms. Different roles (e.g., Admin, User) and policies are applied to secure the endpoints and ensure that only authorized users can access certain features.

### Table-Per-Hierarchy (TPH) with EF Core

- **Inheritance Mapping:** Entity Framework Core's Table-Per-Hierarchy (TPH) inheritance mapping strategy is used to manage different types of users (e.g., HubUser, Client) within a single database table.

### Fluent Entity Configuration

- **Database Configuration:** The database entities are configured using the Fluent API in EF Core, allowing precise control over the database schema, relationships, and constraints.

### Polly

- **Resilience and Fault Handling:** Polly is used to implement retry policies, especially for database migrations, ensuring that the application can recover from transient failures gracefully.

### Fluent Validation

- **Data Validation:** FluentValidation is used to enforce complex validation rules for incoming requests, ensuring that only valid data is processed by the API.

### libphonenumber

- **Phone Number Validation:** The `libphonenumber` package is used to validate and format phone numbers, ensuring that all phone numbers conform to international standards.

### Well-Documented Code

- **Code Documentation:** The entire codebase is thoroughly documented with XML comments. Swagger is configured to include these comments in the API documentation, making it easier for developers to understand and use the API.

### Additional Features

- **Output Caching:** Redis is used for output caching, ensuring that API responses are cached and served efficiently, reducing the load on the server.
- **Global Exception Handling:** A global exception handler is implemented to capture and handle exceptions gracefully, providing meaningful error messages to API consumers.
- **Seed Data:** The application seeds initial data (e.g., admin user, countries, cities) when it runs for the first time, making it easier to get started with the API.

## Usage

Once the application is running, you can explore the API using Swagger UI at `http://localhost:5000`. The Swagger documentation provides detailed information about each endpoint, including the request and response formats, required parameters, and authentication methods. 
If you're more into Postman, I prepared a collection for you here: [Postman Collection](https://github.com/isl-mahrous/FundingSouq.Assessment/blob/master/postman_collection.json)

