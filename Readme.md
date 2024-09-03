# Library Management System

## Overview

The Library Management System is a backend service designed to manage books, borrowers, and borrowing records for a library. The system is composed of a gRPC service that handles the core business logic and a RESTful API that consumes the gRPC service, making it easier for clients to interact with the system.

## Features

- **gRPC Services**: Core functionalities are exposed through a gRPC API.
- **RESTful API**: Provides a more accessible interface for consuming the gRPC services.
- **Book Management**: Track books, their availability, and borrowing history.
- **Borrower Management**: Manage library users and their borrowing activities.
- **Statistics and Metrics**: Calculate reading rates, track the most borrowed books, and more.
- **Database**: Uses SQL Server for data storage, with migrations managed via Entity Framework Core.
- **Testing**: Integration tests using xUnit to validate functionality.

Note to run this try remove the credentials in the project and input your credentials

## Project Structure

```plaintext
├── src
│   ├── Library.Api             # RESTful API for consuming the gRPC services
│   ├── Library.Grpc            # gRPC service implementation
│   ├── Library.Grpc.Contract   # gRPC service contracts (protobuf definitions)
│   ├── Library.DataModel       # Entity Framework Core models and database context
│   ├── Library.Service         # Business logic and service layer
├── tests
│   └── LibraryService.Test     # Integration tests using xUnit
├── docker-compose.yml          # Docker Compose file to run the application with dependencies
└── README.md                   # This file
