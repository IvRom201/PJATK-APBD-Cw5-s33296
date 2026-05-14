# RoomReservationApi

RoomReservationApi is a simple ASP.NET Core Web API for managing rooms and room reservations.

The project demonstrates basic REST API design, controller-based endpoints, model validation, in-memory data storage, filtering, and business rules such as preventing overlapping reservations for the same room.

## Features

### Rooms

- Get all rooms
- Filter rooms by:
  - minimum capacity
  - projector availability
  - active status
- Get a room by ID
- Get rooms by building code
- Create a new room
- Update an existing room
- Delete a room
- Prevent deleting rooms that already have related reservations

### Reservations

- Get all reservations
- Filter reservations by:
  - date
  - status
  - room ID
- Get a reservation by ID
- Create a new reservation
- Update an existing reservation
- Delete a reservation
- Validate that the selected room exists
- Prevent reservations for inactive rooms
- Prevent overlapping reservations for the same room
- Ignore cancelled reservations when checking time conflicts

## Tech Stack

- C#
- ASP.NET Core Web API
- .NET 10
- OpenAPI / Swagger UI
- In-memory data storage

## Project Structure

```txt
RoomReservationApi/
├── Controllers/
│   ├── ReservationsController.cs
│   └── RoomsController.cs
│
├── Data/
│   └── AppData.cs
│
├── Models/
│   ├── Reservation.cs
│   └── Room.cs
│
├── Program.cs
├── appsettings.json
└── RoomReservationApi.csproj


## Data Model

### Room

```json
{
  "id": 1,
  "name": "Aula A101",
  "buildingCode": "A",
  "floor": 1,
  "capacity": 80,
  "hasProjector": true,
  "isActive": true
}
```

### Reservation

```json
{
  "id": 1,
  "roomId": 2,
  "organizerName": "Anna Kowalska",
  "topic": "REST API workshop",
  "date": "2026-05-10",
  "startTime": "10:00:00",
  "endTime": "12:30:00",
  "status": "confirmed"
}
```

Allowed reservation statuses:

```txt
planned
confirmed
cancelled
```

## API Endpoints

### Rooms

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/rooms` | Get all rooms |
| GET | `/api/rooms/{id}` | Get room by ID |
| GET | `/api/rooms/building/{buildingCode}` | Get rooms by building code |
| POST | `/api/rooms` | Create a new room |
| PUT | `/api/rooms/{id}` | Update a room |
| DELETE | `/api/rooms/{id}` | Delete a room |

### Room filters

```http
GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
```

### Reservations

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/reservations` | Get all reservations |
| GET | `/api/reservations/{id}` | Get reservation by ID |
| POST | `/api/reservations` | Create a new reservation |
| PUT | `/api/reservations/{id}` | Update a reservation |
| DELETE | `/api/reservations/{id}` | Delete a reservation |

### Reservation filters

```http
GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
```

## Example Requests

### Create a room

```http
POST /api/rooms
Content-Type: application/json
```

```json
{
  "name": "Lab B204",
  "buildingCode": "B",
  "floor": 2,
  "capacity": 24,
  "hasProjector": true,
  "isActive": true
}
```

### Create a reservation

```http
POST /api/reservations
Content-Type: application/json
```

```json
{
  "roomId": 2,
  "organizerName": "John Smith",
  "topic": "Project meeting",
  "date": "2026-05-15",
  "startTime": "09:00:00",
  "endTime": "10:30:00",
  "status": "planned"
}
```

## Validation Rules

### Room validation

- `name` is required.
- `buildingCode` is required.
- `capacity` must be greater than zero.

### Reservation validation

- `roomId` must be greater than zero.
- `organizerName` is required.
- `topic` is required.
- `date` is required.
- `endTime` must be later than `startTime`.
- `status` must be one of:
  - `planned`
  - `confirmed`
  - `cancelled`

## Business Rules

- A reservation cannot be created for a room that does not exist.
- A reservation cannot be created for an inactive room.
- A reservation cannot overlap with another non-cancelled reservation for the same room on the same date.
- Cancelled reservations are ignored during conflict checking.
- A room cannot be deleted if it has related reservations.

## Getting Started

### Prerequisites

Install the .NET SDK compatible with the target framework used by the project.

### Run the application

Clone the repository:

```bash
git clone https://github.com/IvRom201/RoomReservationApi.git
cd RoomReservationApi
```

Restore dependencies:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run --project RoomReservationApi/RoomReservationApi.csproj
```

After the application starts, open the URL printed in the terminal and go to:

```txt
/swagger
```

The OpenAPI document is available at:

```txt
/openapi/v1.json
```

## Notes

This project currently uses in-memory collections stored in `AppData`.

Data is reset when the application restarts.

The project is suitable as a small educational REST API example and can be extended with:

- Entity Framework Core
- SQL Server or PostgreSQL
- DTOs for request/response models
- Service layer
- Repository layer
- Authentication and authorization
- Global exception handling
- Unit and integration tests
