# Clean Architecture Solution Template for ASP.NET Core

<br>

## Technologies

* .NET 8
* Light Framework
* Entity Framework Core 8
* MediatR
* Mapster
* FluentValidation
* SignalR
* Serilog
* Redis
* RabbitMQ

If you would like to use In Memory Database, you will need to update **WebApi/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": true,
```

Launch the app:
```bash
cd src/WebApi
dotnet run
```

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebApi

This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection.