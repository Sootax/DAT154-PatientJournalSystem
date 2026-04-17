# DAT154 - Assignment 4  
## Digital Patient Journal & Simulation Training System

This project is a distributed .NET system developed for DAT154 as a system architecture assignment.  
The goal is to demonstrate communication between multiple applications, shared architecture, and separation of concerns in a realistic healthcare simulation domain.

## Purpose

The assignment focuses on architecture more than polish.  
This solution is designed as a connected multi-application system where:

- multiple clients communicate through a shared backend
- business logic is reused through shared class libraries
- data contracts are shared between applications
- the system can be extended without major redesign

## Solution Structure

The solution is split into several projects with different responsibilities.

### `Backend.Api`
Central backend for the system.

This project exposes REST endpoints used by the client applications and acts as the communication layer between the different parts of the system. It is responsible for receiving requests, coordinating logic, and returning shared contract models.

#### Main responsibility
- Expose API endpoints for cases, simulation data, interventions, and observations
- Support communication between frontend applications

#### Folders
- `Controllers/`  
  API controllers for handling HTTP requests.


#### Important files
- `Program.cs`  
  Configures the application, services, middleware, and routing.

- `appsettings.json`  
  Application configuration.
---

### `Shared.Application`
Contains shared application logic.

This project holds logic that should not belong directly in the API or UI layers. It acts as the application/service layer and contains reusable business logic for simulation and debriefing.

#### Main responsibility
- Shared business rules
- Simulation logic
- Debrief logic
- Reusable service abstractions

#### Folders
- `Interfaces/`  
  Contracts for services used across the application.

- `Services/`  
  Implementations of application-level services.

#### Important files
- `SimulationEngine.cs`  
  Handles rule-based simulation effects, such as changes in patient vitals after interventions.

- `DebriefService.cs`  
  Handles logic related to post-simulation review and evaluation.

---

### `Shared.Contracts`
Contains shared data contracts between projects.

This project defines the objects exchanged between API and clients. These classes are used to keep communication consistent across applications.

#### Main responsibility
- Shared request models
- Shared response models
- Shared DTOs used across application boundaries

#### Folders
- `Dtos/`  
  Data Transfer Objects used when sending data between layers and applications.

- `Requests/`  
  Models for incoming API requests.

- `Responses/`  
  Models for outgoing API responses.

#### Example files
- `CaseScenarioDto.cs`  
  Represents a patient case scenario.

- `InterventionDto.cs`  
  Represents a registered intervention.

- `VitalSignsDto.cs`  
  Represents patient vital signs.

- `RegisterInterventionRequest.cs`  
  Request model for registering a student intervention.

---

### `Shared.Domain`
Contains the core domain model.

This project defines the system’s core concepts independently of API, UI, or infrastructure details.

#### Main responsibility
- Domain entities
- Domain enums
- Core concepts used throughout the system

#### Folders
- `Entities/`  
  Domain entities such as patient cases, interventions, observations, or simulation sessions.

- `Enums/`  
  Enumerations used in the domain model.

---

### `Shared.Infrastructure`
Contains infrastructure-related code.

This project is intended for technical implementation details that support the application, such as persistence, repositories, or external integrations.

#### Main responsibility
- Database-related code
- Repository implementations
- Infrastructure services
- External system integration

---

### `Simulation.Desktop`
Desktop application used by students during the simulation.

This application loads the active patient case, displays patient status, and allows students to register interventions. It represents the live simulation interface.

#### Main responsibility
- Display active patient case
- Show current and historical vitals
- Register student actions and interventions
- Show live event log or timeline

#### Important files
- `MainWindow.xaml`  
  Main desktop window.

---

### `TeacherAssessment`
Teacher-facing assessment application.

This application is intended for observation during the simulation and structured review afterwards.

#### Main responsibility
- Observe student actions in real time
- Record teacher observations and notes
- Present a debrief view after simulation
- Support evaluation and reporting

#### Folders
- `Components/`  
  Reusable UI components.

- `Pages/`  
  Application pages and routes.

- `Services/`  
  Frontend services for communication and state handling.

- `wwwroot/`  
  Static files such as CSS, JavaScript, and images.

- `Properties/`  
  Project metadata and launch settings.

#### Important files
- `Program.cs`  
  Configures the application and registers services.

- `appsettings.json`  
  Application configuration.

---

### `CaseSetup.Web`
Web application for case creation and case management.

This application is used to create, edit, and activate simulation cases. It is the case setup part of the assignment and is intended for teacher and student use, depending on role.

#### Main responsibility
- Create and edit case scenarios
- View available cases
- Activate a case for simulation use
- Manage scenario data such as patient demographics, diagnoses, medications, allergies, and goals

---

## Architecture

This solution follows a layered and distributed architecture.

### Layer responsibilities
- `Shared.Domain`  
  Contains the core business concepts.

- `Shared.Application`  
  Contains reusable business logic and service-level functionality.

- `Shared.Contracts`  
  Contains transport models used between applications.

- `Shared.Infrastructure`  
  Contains technical implementation details such as persistence.

- `Backend.Api`  
  Exposes the shared backend used by all applications.

- Client applications  
  - `CaseSetup.Web`
  - `Simulation.Desktop`
  - `TeacherAssessment`

These clients communicate with the backend instead of each managing isolated data on their own.

## Communication

The applications are designed to communicate at runtime, which is a core requirement of the assignment.

### Current communication model
- Client applications send requests to `Backend.Api`
- `Backend.Api` uses shared services and models from the shared projects
- Shared DTOs and request models are defined in `Shared.Contracts`
- Shared business logic is placed in `Shared.Application`

## Suggested contributor guidelines

When adding new functionality:

- Put core entities and enums in `Shared.Domain`
- Put reusable business logic in `Shared.Application`
- Put API-facing DTOs, requests, and responses in `Shared.Contracts`
- Put technical persistence code in `Shared.Infrastructure`
- Keep controllers thin in `Backend.Api`
- Keep UI logic inside the relevant frontend project
- Avoid duplicating domain or contract models across projects

## Technologies

This solution is built with .NET and uses a combination of:

- ASP.NET Core Web API
- Web frontend technology for case setup
- Desktop UI technology for simulation
- A separate .NET frontend technology for teacher assessment
- Shared .NET class libraries

## Git Hook Setup

This repository uses a shared Git pre-commit hook to check code formatting before a commit is created.

### Hook location

The hook is stored in:

```text
.githooks/pre-commit
```
### Enable to hook locally
```text
git config core.hooksPath .githooks
```
** What happens after setup
After the hook is configured, it runs automatically on every commit.
If formatting issues are detected:
- the commit is blocked
- you must run `dotnet format`
- then stage the changes and commit again
